using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletController : MonoBehaviour
{
    public int pelletScore = 10;    // Points awarded for eating a pellet
    public int cherryScore = 100;   // Points awarded for eating a cherry

    // Sounds
    public AudioSource NotEating;
    public AudioSource Eating;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check the tag of the object being collided with to determine if it's a pellet or a cherry
            if (gameObject.CompareTag("Pellet"))
            {
                ScoreManager.Instance.AddScore(pelletScore);
                Destroy(gameObject);  // Destroy the pellet
                Eating.Play();
            }else{
                NotEating.Play();
            }
            
            if (gameObject.CompareTag("Cherry"))
            {
                ScoreManager.Instance.AddScore(cherryScore);
                Destroy(gameObject);  // Destroy the cherry
            }
        }
    }
}