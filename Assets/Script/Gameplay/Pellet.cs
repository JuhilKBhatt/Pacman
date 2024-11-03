using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    private PelletAndCherryController PelletAndCherryController;

    void Start()
    {
        // Find the PelletAndCherryController component in the scene
        PelletAndCherryController = FindObjectOfType<PelletAndCherryController>();
        if (PelletAndCherryController == null)
        {
            Debug.LogError("PelletAndCherryController not found in the scene!");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is PacStudent
        if (collision.CompareTag("Player"))
        {
            // Activate Pellet effect through the controller
            PelletAndCherryController.ActivatePellet();

            // Destroy the Pellet after activation
            Destroy(gameObject);
        }
    }
}