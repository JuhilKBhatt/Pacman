using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;   // Reference to Countdown TMPro text
    public TextMeshProUGUI gameTimerText;   // Reference to Timer TMPro text
    public AudioSource backgroundMusic;     // Audio source for the music loop
    public PacStudentController pacStudentController; // Reference to player controller

    public float gameTime;                  // Game timer
    private bool isGameActive = false;      // Track game state

    private static GameManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Disable player and ghost movement at the start
        pacStudentController.enabled = false;

        // Start the countdown
        StartCoroutine(CountdownAndStartGame());
    }

    private IEnumerator CountdownAndStartGame()
    {
        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        // Display "GO!" for 1 second before starting the game
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // Enable game mechanics
        StartGame();
    }
    private void StartGame()
    {
        isGameActive = true;
        gameTime = 0;

        // Enable player and ghost movement
        pacStudentController.enabled = true;

        // Start background music
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }
    private void Update()
    {
        if (isGameActive)
        {
            gameTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(gameTime / 60F);
            int seconds = Mathf.FloorToInt(gameTime % 60F);
            int milliseconds = Mathf.FloorToInt((gameTime * 100F) % 100F);

            gameTimerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }
    }

    public void GameOver()
    {
        string finalTime = gameTimerText.text;

        ScoreManager.Instance.SaveHighScore(ScoreManager.Instance.playerScore, finalTime);

        Debug.Log("Game Over. Final Score: " + ScoreManager.Instance.playerScore);
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}
