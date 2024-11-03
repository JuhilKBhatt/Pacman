using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public float spawnInterval = 10f;
    public float moveSpeed = 3f;

    private Camera mainCamera;
    private float spawnTimer;

    private void Start()
    {
        mainCamera = Camera.main;
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Check if it's time to spawn a new cherry
        if (spawnTimer <= 0f)
        {
            SpawnCherry();
            spawnTimer = spawnInterval;
        }
    }


    // Method to spawn a cherry outside the camera view
    private void SpawnCherry(){
        Vector2 spawnPosition = GetRandomSpawnPositionOutsideCamera();
        GameObject cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        Vector2 targetPosition = GetOppositeSidePosition(spawnPosition);
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

        if (spawnPosition.x < mainCamera.transform.position.x - cameraWidth / 2) // Left side
            return new Vector2(mainCamera.transform.position.x + cameraWidth / 2 + 1, spawnPosition.y);
        else if (spawnPosition.x > mainCamera.transform.position.x + cameraWidth / 2) // Right side
            return new Vector2(mainCamera.transform.position.x - cameraWidth / 2 - 1, spawnPosition.y);
        else if (spawnPosition.y < mainCamera.transform.position.y - cameraHeight / 2) // Bottom side
            return new Vector2(spawnPosition.x, mainCamera.transform.position.y + cameraHeight / 2 + 1);
        else // Top side
            return new Vector2(spawnPosition.x, mainCamera.transform.position.y - cameraHeight / 2 - 1);
    }

    // Coroutine to move the cherry to the target position
    private IEnumerator MoveCherry(GameObject cherry, Vector2 targetPosition)
    {
        // Move the cherry directly to the target position
        while (cherry != null && Vector2.Distance(cherry.transform.position, targetPosition) > 0.1f)
        {
            cherry.transform.position = Vector2.MoveTowards(cherry.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (cherry != null)
        {
            Destroy(cherry);
        }
    }
}