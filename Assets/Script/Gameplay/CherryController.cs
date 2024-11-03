using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;       // Reference to the cherry prefab (2D sprite)
    public float spawnInterval = 10f;     // Time interval for cherry spawn
    public float moveSpeed = 3f;          // Speed of cherry movement

    private Camera mainCamera;
    private float spawnTimer;

    private void Start()
    {
        mainCamera = Camera.main;         // Reference to the main camera
        spawnTimer = spawnInterval;       // Initialize the spawn timer
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Check if it's time to spawn a new cherry
        if (spawnTimer <= 0f)
        {
            SpawnCherry();
            spawnTimer = spawnInterval;   // Reset the timer
        }
    }

    private void SpawnCherry()
    {
        // Determine a random position outside the camera view
        Vector2 spawnPosition = GetRandomSpawnPositionOutsideCamera();

        // Instantiate the cherry and set its initial position
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        // Calculate the target position on the opposite side of the screen
        Vector2 targetPosition = GetOppositeSidePosition(spawnPosition);

        // Start moving the cherry directly to the target position
        StartCoroutine(MoveCherry(cherry, targetPosition));
    }

    private Vector2 GetRandomSpawnPositionOutsideCamera()
    {
        // Calculate the camera bounds
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Define spawn positions outside each side of the camera view in 2D space
        List<Vector2> spawnPositions = new List<Vector2>
        {
            new Vector2(mainCamera.transform.position.x - cameraWidth / 2 - 1, Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2)),
            new Vector2(mainCamera.transform.position.x + cameraWidth / 2 + 1, Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2)),
            new Vector2(Random.Range(mainCamera.transform.position.x - cameraWidth / 2, mainCamera.transform.position.x + cameraWidth / 2), mainCamera.transform.position.y + cameraHeight / 2 + 1),
            new Vector2(Random.Range(mainCamera.transform.position.x - cameraWidth / 2, mainCamera.transform.position.x + cameraWidth / 2), mainCamera.transform.position.y - cameraHeight / 2 - 1)
        };

        // Select a random spawn position
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }

    private Vector2 GetOppositeSidePosition(Vector2 spawnPosition)
    {
        // Calculate the camera bounds
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Determine target position on the opposite side of the screen
        if (spawnPosition.x < mainCamera.transform.position.x - cameraWidth / 2) // Left side
            return new Vector2(mainCamera.transform.position.x + cameraWidth / 2 + 1, spawnPosition.y);
        else if (spawnPosition.x > mainCamera.transform.position.x + cameraWidth / 2) // Right side
            return new Vector2(mainCamera.transform.position.x - cameraWidth / 2 - 1, spawnPosition.y);
        else if (spawnPosition.y < mainCamera.transform.position.y - cameraHeight / 2) // Bottom side
            return new Vector2(spawnPosition.x, mainCamera.transform.position.y + cameraHeight / 2 + 1);
        else // Top side
            return new Vector2(spawnPosition.x, mainCamera.transform.position.y - cameraHeight / 2 - 1);
    }

    private IEnumerator MoveCherry(GameObject cherry, Vector2 targetPosition)
    {
        // Move the cherry directly to the target position
        while (cherry != null && Vector2.Distance(cherry.transform.position, targetPosition) > 0.1f)
        {
            cherry.transform.position = Vector2.MoveTowards(cherry.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Final check before destroying the cherry to prevent errors
        if (cherry != null)
        {
            Destroy(cherry);
        }
    }
}