using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentCollisionHandler : MonoBehaviour
{
    public ParticleSystem collisionEffect;      // Particle effect to be played on collision
    public AudioSource wallCollisionSound;      // Sound to play on wall collision
    
    private PacStudentController pacStudentController;
    private Vector3 lastValidPosition;          // Last valid position before collision

    private void Start()
    {
        pacStudentController = GetComponent<PacStudentController>();
        if (pacStudentController == null)
        {
            Debug.LogError("PacStudentController component not found!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only handle collisions with walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Move PacStudent back to the last valid position
            pacStudentController.StopMovement();
            transform.position = lastValidPosition;

            // Play particle effect at the collision point
            if (collisionEffect != null)
            {
                Vector3 collisionPoint = collision.GetContact(0).point;
                collisionEffect.transform.position = collisionPoint;
                collisionEffect.Play();
            }

            // Play wall collision sound effect
            if (wallCollisionSound != null && !wallCollisionSound.isPlaying)
            {
                wallCollisionSound.Play();
            }
        }
    }

    private void Update()
    {
        // Track the last valid position when PacStudent is not moving
        if (!pacStudentController.isLerping)
        {
            lastValidPosition = transform.position;
        }
    }
}