using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    // PacStudent Speed
    public float speed = 1.5f;
    
    // All Movement Animations
    public Animator animator;

    // Sound
    public AudioSource NotEatting;

    // Path Var for PacStudent
    private Vector2[] path;
    private int currentTarget = 0;

    // Movement radius (2f in all direction)
    public float moveDistance = 3.5f;

    void Start()
    {
        // Get starting position of PacStudent
        Vector2 startPosition = transform.position;

        // Calculate four corners from start position
        Vector2 topLeft = new Vector2(startPosition.x - moveDistance, startPosition.y + moveDistance);
        Vector2 topRight = new Vector2(startPosition.x + moveDistance, startPosition.y + moveDistance);
        Vector2 bottomRight = new Vector2(startPosition.x + moveDistance, startPosition.y - moveDistance);
        Vector2 bottomLeft = new Vector2(startPosition.x - moveDistance, startPosition.y - moveDistance);

        // Define the path
        path = new Vector2[] { topLeft, topRight, bottomRight, bottomLeft };

        transform.position = startPosition;
    }

    void Update()
    {
        MovePacStudent();
    }

    // Move PacStudent along path
    void MovePacStudent()
    {
        // Get positions
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = path[currentTarget];

        // Move towards corner
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        // Rotate PacStudent
        Vector2 direction = (targetPosition - currentPosition).normalized;

        if (direction == Vector2.right) // Moving right
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.up) // Moving up
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction == Vector2.left) // Moving left
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            transform.localScale = new Vector3(1, -1, 1);
        }
        else if (direction == Vector2.down) // Moving down
        {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Check if it's target corner
        if ((Vector2)transform.position == targetPosition)
        {
            // Update the target to the next corner
            currentTarget = (currentTarget + 1) % path.Length;
        }

        if (!NotEatting.isPlaying)
        {
            NotEatting.Play();
        }
    }
}