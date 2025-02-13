using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] public Vector3 checkpointPosition;
    public PlayerMovement PlayerScript;
    [SerializeField] public GameObject PlayerObject;
    [SerializeField] public GameObject CheckpointText;

    /// <summary>
    /// saves the position of the checkpoint for the player to return to.
    /// </summary>
    private void Start()
    {
        PlayerScript = PlayerMovement.Instance;
        checkpointPosition = transform.position;
    }

    /// <summary>
    /// When the player touches the checkpoint, it saves the checkpoint's number for when they die.
    /// </summary>
    /// <param name="Player"></param>
    private void OnTriggerEnter(Collider Player)
    {
        PlayerScript.lastCheckpointActivated = checkpointPosition;
        StartCoroutine(CheckpointTextOnAndOff());
    }
     
    IEnumerator CheckpointTextOnAndOff()
    {
        CheckpointText.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        CheckpointText.SetActive(false);
    }
}
