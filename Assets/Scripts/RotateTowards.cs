using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Visual();
    }

    private void Visual()
    {
        Vector3 dir = target.position - transform.position;

        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        rot.x = 0;
        rot.z = 0;
        
        transform.rotation = rot;
    }
}
