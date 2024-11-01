using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f;         // Speed of movement
    private Vector3 targetPosition;      // Target position for lerping
    private bool isLerping = false;      // Indicates if PacStudent is currently moving
    public Animator animator;            // All Movement Animations

    // Sounds
    public AudioSource NotEating;
    public AudioSource Eating;

    // Particle system for dust effect
    public ParticleSystem dustEffect;    // Reference to the dust effect particle system

    // Track last and current input directions
    private KeyCode lastInput;   
    private KeyCode currentInput; 

    private float cellSize = 1f;         // Size of each cell in world units

    // Offset position of the top-left corner of levelMap in world coordinates
    private Vector3 mapOrigin = new Vector3(-15, 14, 0);

    // Level map
    private int[,] levelMap = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
    };

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

        // Check if target cell is valid (5 or 6)
        if (IsValidMove(newTargetPosition))
        {
            currentInput = direction;
            targetPosition = newTargetPosition;
            isLerping = true;
            PlayMovementAudio();
            PlayDustEffect(); // Trigger the dust effect when starting to move
            RotatePacStudent(direction);
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

    private bool IsValidMove(Vector3 newPosition)
    {
        // Convert world position to grid coordinates using the offset of mapOrigin
        int gridX = Mathf.RoundToInt((newPosition.x - mapOrigin.x) / cellSize);
        int gridY = Mathf.RoundToInt((mapOrigin.y - newPosition.y) / cellSize); // Invert Y due to top-left origin in map

        // Check if grid position is within level bounds
        if (gridX >= 0 && gridX < levelMap.GetLength(1) && gridY >= 0 && gridY < levelMap.GetLength(0))
        {
            int cellValue = levelMap[gridY, gridX];
            return cellValue == 5 || cellValue == 6; // Allow movement only on cells marked as 5 or 6
        }

        return false;
    }

    private void RotatePacStudent(KeyCode direction)
    {
        if (direction == KeyCode.W)
            transform.rotation = Quaternion.Euler(0, 0, 90);    // Up
        else if (direction == KeyCode.A)
            transform.rotation = Quaternion.Euler(0, 0, 180);   // Left
        else if (direction == KeyCode.S)
            transform.rotation = Quaternion.Euler(0, 0, 270);   // Down
        else if (direction == KeyCode.D)
            transform.rotation = Quaternion.Euler(0, 0, 0);     // Right
    }

    private void PlayMovementAudio()
    {
        if (!NotEating.isPlaying)
            NotEating.Play();
        else
            Eating.Play();
    }

    private void PlayDustEffect()
    {
        if (dustEffect != null)
        {
            Vector3 dustOffset = Vector3.zero;
            if (currentInput == KeyCode.W)
                dustOffset = new Vector3(0, -0.5f, 0);
            else if (currentInput == KeyCode.S)
                dustOffset = new Vector3(0, 0.5f, 0);
            else if (currentInput == KeyCode.A)
                dustOffset = new Vector3(0.5f, 0, 0);
            else if (currentInput == KeyCode.D)
                dustOffset = new Vector3(-0.5f, 0, 0);

            dustEffect.transform.position = transform.position + dustOffset;
            dustEffect.transform.rotation = Quaternion.identity;
            dustEffect.Play();
        }
    }
}