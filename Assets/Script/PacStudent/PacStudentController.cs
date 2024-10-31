using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement
    private Vector3 targetPosition;  // Target position for lerping
    private bool isLerping = false;  // Indicates if PacStudent is currently moving
    public Animator animator; // All Movement Animations

    // Sounds
    public AudioSource NotEatting;
    public AudioSource Eatting;

    // Track last and current input directions
    private KeyCode lastInput;   
    private KeyCode currentInput; 

    private float cellSize = 1f; // Size of each cell in world units

    private void Start()
    {
        targetPosition = transform.position; // Start at the current position
    }

    private void Update()
    {
        HandleInput();

        // Move if not currently lerping to a new position
        if (!isLerping)
        {
            TryMove(lastInput);

            if (!isLerping) // If not moving, try moving in the current input direction
            {
                TryMove(currentInput);
            }
        }
        else
        {
            // Smoothly lerp towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            
            // Snap to target position when close enough, and stop lerping
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isLerping = false;
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.A)) lastInput = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) lastInput = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) lastInput = KeyCode.D;
    }

    private void TryMove(KeyCode direction)
    {
        Vector3 newTargetPosition = GetAdjacentWorldPosition(direction);

        // Set the new position as the target if it's different from the current one
        if (newTargetPosition != transform.position)
        {
            currentInput = direction;
            targetPosition = newTargetPosition;
            isLerping = true;
            PlayMovementAudio();
            PlayDustEffect();
        }
    }

    private Vector3 GetAdjacentWorldPosition(KeyCode direction)
    {
        Vector3 offset = Vector3.zero;

        // Adjust the target position based on input direction
        if (direction == KeyCode.W) offset = new Vector3(0, cellSize, 0);
        else if (direction == KeyCode.A) offset = new Vector3(-cellSize, 0, 0);
        else if (direction == KeyCode.S) offset = new Vector3(0, -cellSize, 0);
        else if (direction == KeyCode.D) offset = new Vector3(cellSize, 0, 0);

        return transform.position + offset;
    }

    private void PlayMovementAudio()
    {
        if (!NotEatting.isPlaying)
        {
            NotEatting.Play();
        }
        
        if (!Eatting.isPlaying)
        {
            Eatting.Play();
        }
    }

    private void PlayDustEffect()
    {
        // Placeholder for dust effect logic
    }
}