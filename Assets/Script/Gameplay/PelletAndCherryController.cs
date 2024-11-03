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

    // Method to handle pellet or cherry activation
    public void ActivatePellet(GameObject collectedObject)
    {
        if (collectedObject.CompareTag("Pellet"))
        {
            ScoreManager.Instance.AddScore(pelletScore);

            if (eatingPelletSFX != null)
            {
                eatingPelletSFX.Play();
            }
        }
        else if (collectedObject.CompareTag("Cherry"))
        {
            ScoreManager.Instance.AddScore(cherryScore);

            if (eatingPelletSFX != null)
            {
                eatingPelletSFX.Play();
            }
        }
    }

    // Method to play "not eating" sound
    public void PlayNotEatingSound()
    {
        if (notEatingPelletSFX != null)
        {
            notEatingPelletSFX.Play();
        }
    }
}