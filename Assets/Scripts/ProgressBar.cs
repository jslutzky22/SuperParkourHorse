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
        maxDistance = finish.transform.position.x;
        progressBar.fillAmount = player.transform.position.x / maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.fillAmount < 1)
        {
            progressBar.fillAmount = player.transform.position.x / maxDistance;
        }
        
    }
}
