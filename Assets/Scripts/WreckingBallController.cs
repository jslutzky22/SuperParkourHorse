using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class WreckingBallController : MonoBehaviour
{
    [SerializeField] private bool swinging;
    // Start is called before the first frame update
    void Start()
    {
        swinging = false;
    }

    private void FixedUpdate()
    {
        if (!swinging)
        {
            if (transform.eulerAngles.x == -70)
            {   
                transform.Rotate(1, 0, 0, Space.World);
                StartCoroutine(WaitForSwingToStart());
                //while (transform.eulerAngles.x < 70)
                {
                    transform.Rotate(1, 0, 0, Space.World);
                    StartCoroutine(SwingToPositive());
                }
            }
            if (transform.eulerAngles.x == 70)
            {
                transform.Rotate(-1, 0, 0, Space.World);
                StartCoroutine(WaitForSwingToStart());
                while (transform.eulerAngles.x > -70)
                {
                    StartCoroutine(SwingToNegative());
                }
            }
        }
        if (swinging)
        {
            if (transform.eulerAngles.x == 70)
            {
                transform.Rotate(0, 0, 0, Space.World);
                StartCoroutine(WaitForSwing());
            }
            if (transform.eulerAngles.x == -70)
            {
                transform.Rotate(0, 0, 0, Space.World);
                StartCoroutine(WaitForSwing());
            }
        }
    }

    IEnumerator WaitForSwing()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        swinging = false;
    }
    IEnumerator WaitForSwingToStart()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        swinging = true;
    }
    IEnumerator SwingToPositive()
    {
        transform.Rotate(1, 0, 0, Space.World);
        yield return new WaitForSecondsRealtime(0.1f);
    }
    IEnumerator SwingToNegative()
    {
        transform.Rotate(-1, 0, 0, Space.World);
        yield return new WaitForSecondsRealtime(0.1f);
    }
}
