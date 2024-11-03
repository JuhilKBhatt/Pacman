using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletAndCherryController : MonoBehaviour
{
    public int pelletScore = 10;    // Points awarded for eating a pellet
    public int cherryScore = 100;   // Points awarded for eating a cherry

    // Sounds
    public AudioSource notEatingPelletSFX;
    public AudioSource eatingPelletSFX;

    public void ActivatePellet()
    {
        Debug.Log("ActivatePellet() called.");

        // Check the tag of the object being collided with to determine if it's a pellet or a cherry
        if (gameObject.CompareTag("Pellet"))
        {
            ScoreManager.Instance.AddScore(pelletScore);
            Debug.Log("Pellet collected, adding score: " + pelletScore);

            if (eatingPelletSFX != null)
            {
                Debug.Log("Playing eating pellet sound.");
                eatingPelletSFX.Play();
            }
            else
            {
                Debug.LogWarning("EatingPalletSFX is not assigned!");
            }
        }
        else if (gameObject.CompareTag("Cherry"))
        {
            ScoreManager.Instance.AddScore(cherryScore);
            Debug.Log("Cherry collected, adding score: " + cherryScore);

            if (notEatingPelletSFX != null)
            {
                Debug.Log("Playing not eating pellet sound.");
                notEatingPelletSFX.Play();
            }
            else
            {
                Debug.LogWarning("NotEatingPelletSFX is not assigned!");
            }
        }else {
            Debug.LogWarning("Object is not a pellet or a cherry!");
        }
    }
}