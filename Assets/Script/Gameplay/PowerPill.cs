using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPill : MonoBehaviour
{
    // Reference to the PowerPillController in the scene
    private PowerPillController powerPillController;

    private void Start()
    {
        // Find and reference the PowerPillController in the scene
        powerPillController = FindObjectOfType<PowerPillController>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player") && powerPillController != null)
        {
            // Activate Power Pill effect through the controller
            powerPillController.ActivatePowerPill();

            // Destroy the Power Pill after activation
            Destroy(gameObject);
        }
    }
}