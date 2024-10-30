using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of lerp movement
    private Vector3 targetPosition; // Target position for lerp movement
    private bool isLerping = false; // Track if PacStudent is moving

    private Vector2Int gridPosition; // PacStudent’s current grid position
    public KeyCode lastInput; // Stores the last key pressed
    private KeyCode currentInput; // Current direction PacStudent is moving

    // Level map setup (5 represents walkable, other numbers represent walls or other obstacles)
    private int[,] levelMap = 
    {
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
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private void Start()
    {
        gridPosition = new Vector2Int(2, 2); // Starting grid position
        targetPosition = new Vector3(gridPosition.x, gridPosition.y, 0);
        transform.position = targetPosition; // Initialize position
    }

    private void Update()
    {
        HandleInput();
        
        if (!isLerping)
        {
            TryMove(lastInput);

            if (!isLerping) // If movement didn't start, try moving in currentInput direction
            {
                TryMove(currentInput);
            }
        }
        else
        {
            // Lerp towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Snap to the target
                gridPosition = new Vector2Int((int)targetPosition.x, (int)targetPosition.y);
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
        Vector2Int targetGridPosition = GetAdjacentGridPosition(direction);

        // Check if target position is within bounds and walkable (where 5 is walkable)
        if (IsWalkable(targetGridPosition))
        {
            currentInput = direction;
            targetPosition = new Vector3(targetGridPosition.x, targetGridPosition.y, 0);
            isLerping = true;
            PlayMovementAudio(); // Play movement sound
            PlayDustEffect(); // Play dust effect
        }
    }

    private Vector2Int GetAdjacentGridPosition(KeyCode direction)
    {
        if (direction == KeyCode.W) return gridPosition + Vector2Int.up;
        if (direction == KeyCode.A) return gridPosition + Vector2Int.left;
        if (direction == KeyCode.S) return gridPosition + Vector2Int.down;
        if (direction == KeyCode.D) return gridPosition + Vector2Int.right;
        return gridPosition;
    }

    private bool IsWalkable(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= levelMap.GetLength(0) || gridPos.y >= levelMap.GetLength(1))
            return false; // Out of bounds
        return levelMap[gridPos.y, gridPos.x] == 5; // 5 represents walkable areas
    }

    private void PlayMovementAudio()
    {
        // Placeholder for movement audio logic
    }

    private void PlayDustEffect()
    {
        // Placeholder for dust particle effect logic
    }
}