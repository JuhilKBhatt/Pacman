using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private PelletAndCherryController pelletAndCherryController;
    private GameManager gameManager;

    void Start()
    {
        pelletAndCherryController = FindObjectOfType<PelletAndCherryController>();
        gameManager = FindObjectOfType<GameManager>(); // Get reference to GameManager
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the game is active before processing collision
        if (gameManager != null && gameManager.IsGameActive())
        {
            if (collision.CompareTag("Player"))
            {
                // Check if the current object is a pellet or cherry
                if (gameObject.CompareTag("Pellet") || gameObject.CompareTag("Cherry"))
                {
                    // Pass this pellet/cherry object to ActivatePellet
                    pelletAndCherryController.ActivatePellet(gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}