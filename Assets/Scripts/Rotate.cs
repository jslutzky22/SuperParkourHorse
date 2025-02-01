using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotate : MonoBehaviour
{
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    [SerializeField] private float _speedZ;

    /// <summary>
    /// Makes things spin
    /// </summary>
    void Update()
    {
        transform.Rotate(360 * _speedX * Time.deltaTime, 360 * _speedY * Time.deltaTime, 360 * _speedZ * Time.deltaTime);
    }
}
