using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class WreckingBallController : MonoBehaviour
{
    [SerializeField] private bool swinging;

    private void FixedUpdate()
    {
        if (transform.localEulerAngles.x > 70 && transform.localEulerAngles.x < 180)
        {
            StopCoroutine(PauseBeforeNextSwingOne());
            StopCoroutine(PauseBeforeNextSwingTwo());
            transform.localEulerAngles = new Vector3(69, 0, 0);
            StartCoroutine(PauseBeforeNextSwingOne());
        }
        if (transform.localEulerAngles.x < 290 && transform.localEulerAngles.x > 180)
        {
            StopCoroutine(PauseBeforeNextSwingOne());
            StopCoroutine(PauseBeforeNextSwingTwo());
            transform.localEulerAngles = new Vector3 (291, 0, 0);
            StartCoroutine(PauseBeforeNextSwingTwo());
        }
    }

    IEnumerator PauseBeforeNextSwingOne()
    {
        if (!swinging)
        {
            swinging = true;
            yield return new WaitForSecondsRealtime(1f);
            while (transform.localEulerAngles.x < 70 || transform.localEulerAngles.x > 290)
            {
                transform.localEulerAngles += new Vector3(-1, 0, 0);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            swinging = false;
        }
    }
    IEnumerator PauseBeforeNextSwingTwo()
    {
        if (!swinging)
        {
            swinging = true;
            yield return new WaitForSecondsRealtime(1f);
            while (transform.localEulerAngles.x < 70 || transform.localEulerAngles.x > 290)
            {
                transform.localEulerAngles += new Vector3(1, 0, 0);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            swinging = false;
        }
    }
}