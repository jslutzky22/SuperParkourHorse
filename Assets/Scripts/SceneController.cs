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


    public void ExitButton()
    {
        Application.Quit();
    }
}
