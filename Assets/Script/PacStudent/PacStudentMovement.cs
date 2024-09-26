using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    // Speed
    public float speed = 1.5f;
    
    // movement animations
    public Animator animator;

    // movement sound
    public AudioSource NotEatting;

    // Path for PacStudent to move
    private Vector2[] path;
    private int currentTarget = 0;

    // Movement distance (2 units)
    public float moveDistance = 2f;

    void Start()
    {
        // Get the starting position of PacStudent
        Vector2 startPosition = transform.position;

        // Calculate the four corners relative to the start position
        Vector2 topLeft = new Vector2(startPosition.x - moveDistance, startPosition.y + moveDistance);
        Vector2 topRight = new Vector2(startPosition.x + moveDistance, startPosition.y + moveDistance);
        Vector2 bottomRight = new Vector2(startPosition.x + moveDistance, startPosition.y - moveDistance);
        Vector2 bottomLeft = new Vector2(startPosition.x - moveDistance, startPosition.y - moveDistance);

        // Define the clockwise path
        path = new Vector2[] { topLeft, topRight, bottomRight, bottomLeft };

        // Set the initial position
        transform.position = startPosition;
    }

    void Update()
    {
        MovePacStudent();
    }

    // Move PacStudent along the path
    void MovePacStudent()
    {
        // Get the current position
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = path[currentTarget];

        // Move towards the current target corner
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // Rotate PacStudent based on the direction of movement
        Vector2 direction = (targetPosition - currentPosition).normalized;

        if (direction == Vector2.right) // Moving right
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // z = 0
            transform.localScale = new Vector3(1, 1, 1);    // Normal scale
        }
        else if (direction == Vector2.up) // Moving up
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); // z = 90
            transform.localScale = new Vector3(1, 1, 1);     // Normal scale
        }
        else if (direction == Vector2.left) // Moving left
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); // z = 180
            transform.localScale = new Vector3(1, -1, 1);     // Flip on Y-axis
        }
        else if (direction == Vector2.down) // Moving down
        {
            transform.rotation = Quaternion.Euler(0, 0, 270); // z = 270
            transform.localScale = new Vector3(1, 1, 1);      // Normal scale
        }

        // Check if PacStudent has reached the target corner
        if ((Vector2)transform.position == targetPosition)
        {
            // Update the target to the next corner
            currentTarget = (currentTarget + 1) % path.Length;
        }

        // Play movement animation if it's not already playing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PacStudentIdle"))
        {
            animator.Play("PacStudentIdle");
        }

        // Play movement audio if it's not playing
        if (!NotEatting.isPlaying)
        {
            NotEatting.Play();
        }
    }
}