using UnityEngine;
using TMPro;

public class PelletController : MonoBehaviour
{
    public int pelletScore = 10;        // Points awarded for eating a pellet
    public int cherryScore = 100;       // Points awarded for eating a cherry
    public TextMeshProUGUI scoreText;   // Reference to the score UI element
    private int playerScore = 0;        // Player's current score

    private void Start()
    {
        Debug.Log("PelletController started. Initializing score display.");
        UpdateScoreUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collided with an object: " + gameObject.name);

            // Check if the collided object is a pellet or a cherry
            if (gameObject.CompareTag("Pellet"))
            {
                Debug.Log("Player ate a pellet. Adding " + pelletScore + " points.");
                AddScore(pelletScore);
                Destroy(gameObject);  // Destroy the pellet
                Debug.Log("Pellet destroyed.");
            }
            else if (gameObject.CompareTag("Cherry"))
            {
                Debug.Log("Player ate a cherry. Adding " + cherryScore + " points.");
                AddScore(cherryScore);
                Destroy(gameObject);  // Destroy the cherry
                Debug.Log("Cherry destroyed.");
            }
        }
        else
        {
            Debug.Log("Collision detected with non-player object: " + other.gameObject.name);
        }
    }

    private void AddScore(int points)
    {
        playerScore += points;
        Debug.Log("Score updated. New score: " + playerScore);
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.ToString();
            Debug.Log("Score UI updated to: " + playerScore);
        }
        else
        {
            Debug.LogWarning("Score Text UI element is not assigned!");
        }
    }
}