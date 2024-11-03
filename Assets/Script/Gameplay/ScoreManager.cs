using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int playerScore = 0;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points){
        playerScore += points;
        UpdateScoreUI();
    }

    // Update the score UI text
    private void UpdateScoreUI(){
        scoreText.text = "Score: " + playerScore.ToString();
    }

    // Save the high score if it is greater than the current high score
    public void SaveHighScore(int score, string timeString){
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // If the current score is higher than the high score, save the new high score
        if (score > highScore){
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.SetString("HighScoreTime", timeString);
            PlayerPrefs.Save();
        }
    }
}