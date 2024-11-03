using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private PelletAndCherryController pelletAndCherryController;

    void Start()
    {
        pelletAndCherryController = FindObjectOfType<PelletAndCherryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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