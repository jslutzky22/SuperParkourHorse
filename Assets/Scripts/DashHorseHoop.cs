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
        PlayerScript.rb.velocity = new Vector3(0, 0, 0);
        PlayerScript._moveDirection = new Vector3(0, 0, 0);
        playerCamera = GameObject.FindWithTag("MainCamera");
        playerCamera.transform.rotation = hoopContainer.transform.rotation;
        PlayerScript.cutsceneMode = true;
        PlayerScript.StopSwing();
        repeatCount = 0;
        PlayerScript.rb.mass = 0;
        PlayerScript.transform.position = hoopForward.transform.position;
        while (repeatCount < 10)
        {
            PlayerScript.transform.position += hoopForward.transform.localPosition;
            yield return new WaitForSecondsRealtime(0.05f);
            repeatCount++;
        }
        PlayerScript.rb.mass = 2;
        PlayerScript.cutsceneMode = false;
        hoopRunning = false;
    }
}
