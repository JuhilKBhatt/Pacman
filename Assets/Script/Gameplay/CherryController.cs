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

        // Start moving the cherry toward the center of the level
        StartCoroutine(MoveCherry(cherry));
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

    private IEnumerator MoveCherry(GameObject cherry)
    {
        // Set the target position to the center of the level
        Vector2 targetPosition = Vector2.zero;

        while (cherry != null)
        {
            // Lerp the cherry's position toward the center of the screen
            cherry.transform.position = Vector2.MoveTowards(cherry.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the cherry has reached the target
            if (Vector2.Distance(cherry.transform.position, targetPosition) < 0.1f)
            {
                Destroy(cherry); // Destroy the cherry when it reaches the target
                yield break;
            }

            yield return null;
        }
    }
}