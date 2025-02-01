using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] public Vector3 checkpointPosition;
    [SerializeField] public PlayerMovement PlayerScript;

    /// <summary>
    /// saves the position of the checkpoint for the player to return to.
    /// </summary>
    private void Start()
    {
        checkpointPosition = transform.position;
    }

    /// <summary>
    /// When the player touches the checkpoint, it saves the checkpoint's number for when they die.
    /// </summary>
    /// <param name="Player"></param>
    private void OnTriggerEnter(Collider Player)
    {
        PlayerScript.lastCheckpointActivated = checkpointPosition;
    }
}
