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

    public void LoadSmVerticalCup()
    {
        SceneManager.LoadScene("SmVerticalRace");

    }

    public void LoadSmHorizontalCup()
    {
        SceneManager.LoadScene("SmHorizontalTrack");

    }

    public void LoadSmPreciseCup()
    {
        SceneManager.LoadScene("SmTheFinalTrack");

    }

    public void LoadVerticalCup()
    {
        SceneManager.LoadScene("VerticalRace");

    }

    public void LoadHorizontalCup()
    {
        SceneManager.LoadScene("HorizontalTrack");

    }

    public void LoadPreciseCup()
    {
        SceneManager.LoadScene("TheFinalTrack");

    }


    public void ExitButton()
    {
        Application.Quit();
    }
}
