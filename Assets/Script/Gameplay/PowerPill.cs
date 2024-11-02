using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPill : MonoBehaviour
{
    private PowerPillController powerPillController;

    private void Start()
    {
        // Find and reference the PowerPillController in the scene
        powerPillController = FindObjectOfType<PowerPillController>();

        if (powerPillController == null)
        {
            Debug.LogError("PowerPillController not found in the scene. Make sure it's attached to an active GameObject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is PacStudent
        if (collision.CompareTag("Player") && powerPillController != null)
        {
            // Activate Power Pill effect through the controller
            powerPillController.ActivatePowerPill();

            // Destroy the Power Pill after activation
            Destroy(gameObject);
        }
    }
}