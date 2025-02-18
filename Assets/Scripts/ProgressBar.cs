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
    [SerializeField] private bool verticalRace;
    [SerializeField] private bool zAxisRace;
   
    // Start is called before the first frame update
    void Start()
    {
        if (verticalRace & zAxisRace == false)
        {
            maxDistance = finish.transform.position.y;
            progressBar.fillAmount = player.transform.position.y / maxDistance;
        }
        if (verticalRace == false & zAxisRace == false)
        {
            maxDistance = finish.transform.position.x;
            progressBar.fillAmount = player.transform.position.x / maxDistance;
        }
        if (zAxisRace == true)
        {
            maxDistance = finish.transform.position.z;
            progressBar.fillAmount = player.transform.position.z / maxDistance;
        }
    }

    //absolute value might not be necessary idk

    // Update is called once per frame
    void Update()
    {
        if (progressBar.fillAmount < 1 && verticalRace == false & zAxisRace == false)
        {
            progressBar.fillAmount = player.transform.position.x / maxDistance;
        }
        if (progressBar.fillAmount < 1 && verticalRace == true & zAxisRace == false)
        {
            progressBar.fillAmount = player.transform.position.y / maxDistance;
        }
        if (progressBar.fillAmount < 1 && zAxisRace == true)
        {
            progressBar.fillAmount = player.transform.position.z / maxDistance;
        }
    }
}
