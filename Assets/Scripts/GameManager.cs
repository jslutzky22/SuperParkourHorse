using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownText;
    public GameObject countdownTextSprite;

    public float currentTime;
    public float raceTime;
    [SerializeField] private Image enemyProgressBar;

    private bool countUp = true;
    public bool gameStarted;
    public bool gameFinished;
    public static bool gameIsPaused = false;

    public PlayerInput myPlayerInput;
    private InputAction pauseAction;

    [SerializeField]
    private GameObject pauseMenuUI;

    public PlayerMovement PlayerScript;

    // Timer & Countdown
    void Start()
    {
        PlayerScript = PlayerMovement.Instance;

        myPlayerInput.currentActionMap.Enable();
        pauseAction = myPlayerInput.currentActionMap.FindAction("Pause");

        pauseAction.started += Pause_started;

        enemyProgressBar.fillAmount = currentTime / raceTime;

        StartCoroutine(CountdownCoroutine());

    }

    void OnDestroy() 
    
         {
        pauseAction.started -= Pause_started;
    }

    IEnumerator CountdownCoroutine()
    {
        PlayerScript.cutsceneMode = true;
        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "Go!";
        gameStarted = true;
        PlayerScript.cutsceneMode = false;




        yield return new WaitForSeconds(1.0f);
        countdownText.text = "";
        countdownTextSprite.SetActive(false);
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
        if (enemyProgressBar.fillAmount < 1)
        {
            enemyProgressBar.fillAmount = currentTime / raceTime;
        }

        if (currentTime >= raceTime)
        {
            LoadLoseScene();
        }

    }

    public void LoadLoseScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Lose Scene");
        Debug.Log("You've Lost");

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
            //Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Resume();
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }


    // Pause Controls
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameIsPaused = true;


    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start Menu");

    }


}
