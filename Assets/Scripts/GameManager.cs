using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownText;

    public float currentTime;

    public bool countUp;
    public bool gameStarted;
    public bool gameFinished;
    public static bool gameIsPaused = false;

    public PlayerInput myPlayerInput;
    private InputAction pauseAction;


    public GameObject pauseMenuUI;

    private void Awake()
    {
        myPlayerInput.currentActionMap.Enable();
        pauseAction = myPlayerInput.currentActionMap.FindAction("Pause");

        pauseAction.started += Pause_started;

    }

    // Timer & Countdown
    void Start()
    {
        StartCoroutine(CountdownCoroutine());

    }

    IEnumerator CountdownCoroutine()
    {

        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "Go!";
        gameStarted = true;




        yield return new WaitForSeconds(1.0f);
        countdownText.text = "";
        yield return null;
    }

    void Update()
    {
        if (gameStarted)
        {
            currentTime = countUp ? currentTime += Time.deltaTime : currentTime -= Time.deltaTime;
            SetTimerText();

        }
        else if (gameFinished)
        {
            gameStarted = false;

        }


    }

    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("00.00");

    }



    private void Pause_started(InputAction.CallbackContext context)
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Pause();

        }
        else
        {
            Resume();
        }
    }


    // Pause Controls
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;


    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

    }

    public void RestartGame()
    {
        // Resetting necessary game variables
        currentTime = 0;
        gameStarted = false;
        gameFinished = false;
        gameIsPaused = false;

        // Resuming time in case the game is paused
        Time.timeScale = 1f;

        // Reloading the active scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
