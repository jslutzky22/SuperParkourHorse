using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    
    /// <summary>
    /// Makes the horse face away from the camera.
    /// </summary>
    void Update()
    {
        transform.rotation = Camera.transform.rotation;
    }
}
