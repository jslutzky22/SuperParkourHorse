using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float LRspeed;
    [SerializeField] private float FBspeed;
    [SerializeField] private bool right;
    [SerializeField] private bool forward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (right)
        {
            transform.position += Vector3.right * LRspeed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.right * LRspeed * Time.deltaTime;
        }
        if (forward)
        {
            transform.position += Vector3.forward * FBspeed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.forward * FBspeed * Time.deltaTime;
        }
    }
}
