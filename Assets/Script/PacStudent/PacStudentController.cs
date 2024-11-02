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

    // Define the base map (top-left quadrant) to be mirrored
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

    private int[,] CreateFourQuadrantMap(int[,] baseMap)
    {
        int rows = baseMap.GetLength(0);
        int cols = baseMap.GetLength(1);

        int[,] fullMap = new int[rows * 2, cols * 2];

        // Fill the four quadrants
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Top-left quadrant (original map)
                fullMap[i, j] = baseMap[i, j];

                // Top-right quadrant (horizontal mirror)
                fullMap[i, cols * 2 - j - 1] = baseMap[i, j];

                // Bottom-left quadrant (vertical mirror)
                fullMap[rows * 2 - i - 2, j] = baseMap[i, j];

                // Bottom-right quadrant (horizontal + vertical mirror)
                fullMap[rows * 2 - i - 2, cols * 2 - j - 1] = baseMap[i, j];
            }
        }

        return fullMap;
    }

    private Vector2Int startingGridPosition = new Vector2Int(1, 0); // Second row, first column
    private Vector3 gridOrigin = new Vector3(-15, 14, 0);           // World origin for the grid


    private void Start()
    {
        // Generate the full 4-quadrant map
        int[,] fullMap = CreateFourQuadrantMap(levelMap);
        levelMap = fullMap;
        // Calculate PacStudent's starting position
        targetPosition = GetWorldPositionFromGrid(startingGridPosition);
    }

    private void Update()
    {
        HandleInput();

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

        if (IsValidMove(newTargetPosition))
        {
            currentInput = direction;
            targetPosition = newTargetPosition;
            isLerping = true;
            PlayMovementAudio();
            PlayDustEffect();
            RotatePacStudent(direction);
        }
    }

    private Vector3 GetAdjacentWorldPosition(KeyCode direction)
    {
        Vector2Int offset = Vector2Int.zero;

        if (direction == KeyCode.W) offset = new Vector2Int(0, -1);
        else if (direction == KeyCode.A) offset = new Vector2Int(-1, 0);
        else if (direction == KeyCode.S) offset = new Vector2Int(0, 1);
        else if (direction == KeyCode.D) offset = new Vector2Int(1, 0);

        Vector2Int gridPosition = GetGridPositionFromWorld(transform.position);
        Vector2Int newGridPosition = gridPosition + offset;
        
        return GetWorldPositionFromGrid(newGridPosition);
    }

    private bool IsValidMove(Vector3 newPosition)
    {
        Vector2Int gridPosition = GetGridPositionFromWorld(newPosition);

        if (gridPosition.x >= 0 && gridPosition.x < levelMap.GetLength(1) && gridPosition.y >= 0 && gridPosition.y < levelMap.GetLength(0))
        {
            int cellValue = levelMap[gridPosition.y, gridPosition.x];
            return cellValue == 5 || cellValue == 6 || cellValue == 0;
        }

        return false;
    }

    private Vector2Int GetGridPositionFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((gridOrigin.y - worldPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }

    private Vector3 GetWorldPositionFromGrid(Vector2Int gridPosition)
    {
        float worldX = gridOrigin.x + gridPosition.x * cellSize;
        float worldY = gridOrigin.y - gridPosition.y * cellSize;
        return new Vector3(worldX, worldY, 0);
    }

    private void RotatePacStudent(KeyCode direction)
    {
        // Rotate PacStudent based on movement direction
        if (direction == KeyCode.W)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); // Up
            transform.localScale = new Vector3(1, 1, 1); // Reset scale for correct orientation
        }
        else if (direction == KeyCode.A)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Left (rotation remains 0)
            transform.localScale = new Vector3(-1, 1, 1); // Flip on X-axis
        }
        else if (direction == KeyCode.S)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270); // Down
            transform.localScale = new Vector3(1, 1, 1); // Reset scale for correct orientation
        }
        else if (direction == KeyCode.D)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Right
            transform.localScale = new Vector3(1, 1, 1); // Reset scale for correct orientation
        }
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