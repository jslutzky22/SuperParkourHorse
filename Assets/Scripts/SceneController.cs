using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");

    }

    public void LoadVerticalCup()
    {
        SceneManager.LoadScene("VerticalRace");

    }

    public void LoadHorizontalCup()
    {
        SceneManager.LoadScene("Horizontal");

    }

    public void LoadPreciseCup()
    {
        SceneManager.LoadScene("Precise");

    }


    public void ExitButton()
    {
        Application.Quit();
    }
}
