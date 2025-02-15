using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHorseHoop : MonoBehaviour
{
    public PlayerMovement PlayerScript;
    [SerializeField] private GameObject hoopForward;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] private GameObject hoopContainer;
    [SerializeField] private int repeatCount;
    [SerializeField] private Vector3 hoopTeleportPoints;
    [SerializeField] private bool hoopRunning;

    private void Start()
    {
        PlayerScript = PlayerMovement.Instance;
    }

    private void OnTriggerEnter(Collider Player)
    {
        if (!hoopRunning)
        {
            StartCoroutine(DashHoopLaunch());
        }
    }

    IEnumerator DashHoopLaunch()
    {
        hoopRunning = true;
        playerCamera = GameObject.FindWithTag("MainCamera");
        playerCamera.transform.rotation = hoopContainer.transform.rotation;
        repeatCount = 0;
        //PlayerScript.rb.useGravity = false;
        //hoopTeleportPoints = transform.position;
        //while (repeatCount < 10)
        //{
        //    PlayerScript.transform.position = hoopTeleportPoints;
        //   hoopTeleportPoints += (transform.localPosition + new Vector3(-0.5f, 0, 0));
        //    yield return new WaitForSecondsRealtime(0.05f);
        //    repeatCount++;
        //}
        //PlayerScript.rb.useGravity = true;
        PlayerScript.rb.velocity += hoopForward.transform.localPosition;
        PlayerScript.rb.useGravity = false;
        yield return new WaitForSecondsRealtime(0.5f);
        PlayerScript.rb.useGravity = true;
        hoopRunning = false;
    }
}
