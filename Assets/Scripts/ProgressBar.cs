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
   
    // Start is called before the first frame update
    void Start()
    {
        if (verticalRace == false)
        {
            maxDistance = finish.transform.position.x;
            progressBar.fillAmount = player.transform.position.x / maxDistance;
        }
        if (verticalRace == true)
        {
            maxDistance = finish.transform.position.y;
            progressBar.fillAmount = player.transform.position.y / maxDistance;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.fillAmount < 1 && verticalRace == false)
        {
            progressBar.fillAmount = player.transform.position.x / maxDistance;
        }
        if (progressBar.fillAmount < 1 && verticalRace == true)
        {
            progressBar.fillAmount = player.transform.position.y / maxDistance;
        }
    }
}
