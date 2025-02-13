using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject finish;

    [SerializeField] private Image progressBar;
    [SerializeField] private float maxDistance;
    [SerializeField] private float raceTime;
   
    // Start is called before the first frame update
    void Start()
    {
        maxDistance = Mathf.Abs(finish.transform.position.x) + Mathf.Abs(finish.transform.position.y) + 
            Mathf.Abs(finish.transform.position.z);
        progressBar.fillAmount = (Mathf.Abs(player.transform.position.x) + Mathf.Abs(player.transform.position.y) +
            Mathf.Abs(player.transform.position.z))  / maxDistance;
    }

    //absolute value might not be necessary idk

    // Update is called once per frame
    void Update()
    {
        if (progressBar.fillAmount < 1)
        {
            progressBar.fillAmount = (Mathf.Abs(player.transform.position.x) + Mathf.Abs(player.transform.position.y) +
    Mathf.Abs(player.transform.position.z)) / maxDistance;
        }
    }
}
