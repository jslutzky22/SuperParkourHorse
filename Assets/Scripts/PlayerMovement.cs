using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private Vector3 _moveDirection;
    PlayerInput playerInput;
    InputAction moveAction;
    private float vertical;
    private float horizontal;
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float speedMult;
    [SerializeField] private float speedBase;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float xVelocity;
    [SerializeField] private float zVelocity;
    bool readyToJump = true;
    [SerializeField] private float accelerationAmount;
    [Header("Grapple Stuff")]
    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform gunTip, cam, player;
    //[SerializeField] private Transform combatLookAt;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;
    private Vector3 currentGrapplePosition;
    [SerializeField] private float swingSpeed;
    bool swinging;
    [Header("AirMovement")]
    [SerializeField] private float horizontalThrustForce;
    [SerializeField] private float forwardThrustForce;
    [SerializeField] private float extendCableSpeed;
    [Header("Prediction")]
    [SerializeField] private RaycastHit predictionHit;
    [SerializeField] private float predictionSphereCastRadius;
    [SerializeField] private Transform predictionPoint;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool grounded;
    bool shortenCable;

    [Header("Checkpoints")]
    public Vector3 lastCheckpointActivated;
    private bool resetPressed;
    public static PlayerMovement Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        SpeedControl();
        if (joint != null)
        {
            AirMovement();
        }
        CheckForSwingPoints();
        //Debug.Log(grounded);
        //Debug.Log(horizontal);
        //Debug.Log(vertical);
        xVelocity = Mathf.Abs(rb.velocity.x);
        zVelocity = Mathf.Abs(rb.velocity.z);

        if (xVelocity < 0.5f && xVelocity > 0)
        {
            xVelocity = 0.5f;
        }
        if (zVelocity < 0.5f && zVelocity > 0)
        {
            zVelocity = 0.5f;
        }

        currentSpeed = xVelocity + zVelocity;
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (readyToJump && grounded)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            if (swinging)
            {
                shortenCable = true;
            }
        }
        if (context.canceled)
        {
            shortenCable = false;
        }

    }

    public void Restart(InputAction.CallbackContext context)
    {
        if (resetPressed == false)
        {
            //resetPressed = true; With the way things are coded, we can't have a true restart
            LoadToCheckpoint();
        }
        
    }

    public void Quit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    public void Swing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartSwing();
            Debug.Log("swingShot");
        }
        if (context.canceled)
        {
            StopSwing();
        }
    }

    void MovePlayer()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());
        //rb.velocity = new Vector3(horizontal * moveSpeed, rb.velocity.y, vertical * moveSpeed);
        _moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        if(grounded)
        {
            accelerationAmount = (currentSpeed * speedMult) + speedBase;

            if (accelerationAmount < 0.001)
            {
                accelerationAmount = 0.001f;
            }

            rb.AddForce(_moveDirection.normalized * accelerationAmount * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            accelerationAmount = (currentSpeed * speedMult) + speedBase;

            if (accelerationAmount < 0.001)
            {
                accelerationAmount = 0.001f;
            }

            rb.AddForce(_moveDirection.normalized * accelerationAmount * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        //rb.AddForce(_moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //for speedmult it cannot go at or below 0.05, 0.06 or above works
        //speedmult is the percentage of current speed added to current speed to add acceleration
        //speedbase is the amount added to currentspeed so it doesnt multiply everything by 0
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.y, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    //Limits players max speed to be below the hypothetical max.
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        maxSpeed = 30f;

        if(flatVel.magnitude > maxSpeed)
        {
            if (swinging == false)
            {
                Vector3 limitedVel = flatVel.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    
    private void CheckForSwingPoints()
    {
        {
            if (joint != null) return;

            RaycastHit sphereCastHit;
            RaycastHit raycastHit;

            // Get screen center position
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f + 20f);

            // Create a ray from the camera through the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            // Debugging: Draw the ray in Scene view
            Debug.DrawRay(ray.origin, ray.direction * maxSwingDistance, Color.green, 2f);

            // SphereCast for better detection
            Physics.SphereCast(ray.origin, predictionSphereCastRadius, ray.direction, out sphereCastHit, maxSwingDistance, whatIsGrappleable);

            // Raycast for direct line of sight
            Physics.Raycast(ray, out raycastHit, maxSwingDistance, whatIsGrappleable);

            Vector3 realHitPoint;

            // Direct Hit
            if (raycastHit.point != Vector3.zero)
            {
                realHitPoint = raycastHit.point;
            }
            // Indirect (predicted) hit
            else if (sphereCastHit.point != Vector3.zero)
            {
                realHitPoint = sphereCastHit.point;
            }
            else
            {
                realHitPoint = Vector3.zero;
            }

            // Hit was found
            if (realHitPoint != Vector3.zero)
            {
                predictionPoint.gameObject.SetActive(true);
                predictionPoint.position = realHitPoint;
            }
            else
            {
                predictionPoint.gameObject.SetActive(false);
            }

            predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
        }

    }
    private void StartSwing()
    {
        if (predictionHit.point == Vector3.zero) return;
        //RaycastHit hit;
        //Debug.DrawRay(player.position, cam.forward * maxSwingDistance, Color.red, 2f);
        //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); works

        //Debug.DrawRay(ray.origin, ray.direction * maxSwingDistance, Color.green, 2f);

        //if (Physics.Raycast(player.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        //if (Physics.Raycast(player.position, cam.forward, out hit, maxSwingDistance))
        //Vector3 grappleDirection = (combatLookAt.position - gunTip.position).normalized;
        //if (Physics.Raycast(gunTip.position, grappleDirection, out hit, maxSwingDistance, whatIsGrappleable))
        //if (Physics.Raycast(player.position, cam.forward, out hit, maxSwingDistance))
        //if (Physics.Raycast(ray, out hit, maxSwingDistance, whatIsGrappleable)) works
        //if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
       // {
            swinging = true;
            moveSpeed = swingSpeed;
            //Debug.Log("Grapple hit: " + hit.collider.gameObject.name);

            //swingPoint = hit.point;
            swingPoint = predictionHit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            //Distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Variables that will make adjust the feel of the swing
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            Debug.Log("Line renderer enabled: " + (lr.positionCount > 0));
            if (joint == null)
            {
                Debug.LogError("SpringJoint not created!");
            }

        //}
        //else
        //{
            //Debug.Log("No grapple hit.");
        //}
    }

    private void AirMovement()
    {
        if (horizontal == 1) //Pressing Right
        {
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (horizontal == -1) //Pressing left
        {
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        }
        if (vertical == 1) //Pressing Forward
        {
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);
        }
        if (shortenCable == true)
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        if (vertical == -1) //Pressing backward
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }
    private void StopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
        swinging = false;
        moveSpeed = defaultSpeed;
    }
    private void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
        Debug.Log("LineRenderer status: " + lr.positionCount);

    }

    /// <summary>
    /// Loads the player to their last checkpoint if they touch the KillPlane.
    /// </summary>
    /// <param name="KillPlane"></param>
    private void OnTriggerEnter(Collider KillPlane)
    {
        if (KillPlane.gameObject.tag == "KillPlane")
        {
            LoadToCheckpoint();
        }

        if (KillPlane.gameObject.tag == "Goal")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }

    }

   

    /// <summary>
    /// Called when the player falls onto the killplane (or if we implement a last checkpoint button, there too) to 
    /// return them to the checkpoint. This does not reset anything in the level, and keeps the timer counting up so 
    /// players cannot just keep dying to reset the timer.
    /// </summary>
    private void LoadToCheckpoint()
    {
        transform.position = this.lastCheckpointActivated + new Vector3 (0,1,0);
    }
}
