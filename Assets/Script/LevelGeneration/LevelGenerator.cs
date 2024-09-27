using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // A reference to the sprites
    public Sprite outsideCornerSprite; // 1
    public Sprite outsideWallSprite; // 2
    public Sprite insideCornerSprite; // 3
    public Sprite insideWallSprite; // 4
    public Sprite standardPelletSprite; // 5
    public Sprite powerPelletSprite; // 6
    public Sprite tJunctionSprite; // 7

    // The level layout (Top Left Only)
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

    /// The level Rotation (Top Left Only)
    private float[,] rotationMap = 
    {
        {90, 270, 270, 270, 270, 270, 270, 270, 270, 270, 270, 270, 270, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180},
        {0, 0, 90, 90, 90, 0, 0, 90, 90, 90, 90, 0, 0, 180},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 180},
        {0, 0, 180, 90, 90, 270, 0, 180, 90, 90, 90, 270, 0, 180},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 90, 90, 90, 0, 0, 90, 0, 0, 90, 90, 90, 90},
        {0, 0, 180, 90, 90, 270, 0, 0, 0, 0, 180, 90, 90, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {180, 90, 90, 90, 90, 0, 0, 0, 180, 90, 90, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 90, 90, 90, 270, 0, 180},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 90, 90, 0},
        {90, 90, 90, 90, 90, 270, 0, 180, 270, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    };

    public float tileSize = 1.0f;

    void Start()
    {
        // Delete existing level
        ClearExistingLevel();

        // Procedurally generate level
        GenerateLevel();
    }

    void ClearExistingLevel()
    {
        GameObject[] existingTiles = GameObject.FindGameObjectsWithTag("MapTile");
        foreach (GameObject tile in existingTiles)
        {
            Destroy(tile);
        }
    }

    void GenerateLevel()
    {
        int originalHeight = levelMap.GetLength(0);
        int originalWidth = levelMap.GetLength(1);

        // Full 2-Dimensions
        int fullHeight = originalHeight * 2;
        int fullWidth = originalWidth * 2;

        // Set Size for new 2D array to store full map
        int[,] fullLevelMap = new int[fullHeight, fullWidth];
        float[,] fullRotationMap = new float[fullHeight, fullWidth];

        // Fill top left
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                fullLevelMap[y, x] = levelMap[y, x];
                fullRotationMap[y, x] = rotationMap[y, x]; 
            }
        }

        // Fill top right
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                fullLevelMap[y, fullWidth - 1 - x] = levelMap[y, x];
                fullRotationMap[y, fullWidth - 1 - x] = (rotationMap[y, x] + 180) % 360;
            }
        }

        // Fill bottom left
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                fullLevelMap[fullHeight - 2 - y, x] = levelMap[y, x];
                fullRotationMap[fullHeight - 2 - y, x] = (rotationMap[y, x] + 180) % 360;
            }
        }

        // Fill bottom right
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                fullLevelMap[fullHeight - 2 - y, fullWidth - 1 - x] = levelMap[y, x];
                fullRotationMap[fullHeight - 2 - y, fullWidth - 1 - x] = rotationMap[y, x];
            }
        }

        // Iterate fullLevelMap array and generate the tiles
        for (int y = 0; y < fullHeight; y++)
        {
            for (int x = 0; x < fullWidth; x++)
            {
                // Getimg current position
                int tileType = fullLevelMap[y, x];

                // Determine which sprite to use
                Sprite selectedSprite = null;
                switch (tileType)
                {
                    case 0:
                        selectedSprite = null;
                        break;
                    case 1:
                        selectedSprite = outsideCornerSprite;
                        break;
                    case 2:
                        selectedSprite = outsideWallSprite;
                        break;
                    case 3:
                        selectedSprite = insideCornerSprite;
                        break;
                    case 4:
                        selectedSprite = insideWallSprite;
                        break;
                    case 5:
                        selectedSprite = standardPelletSprite;
                        break;
                    case 6:
                        selectedSprite = powerPelletSprite;
                        break;
                    case 7:
                        selectedSprite = tJunctionSprite;
                        break;
                }

                // Instantiate a new tile
                if (selectedSprite != null)
                {
                    GameObject newTile = new GameObject("Tile_" + x + "_" + y);
                    SpriteRenderer spriteRenderer = newTile.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = selectedSprite;
                    newTile.transform.position = new Vector3(x * tileSize, -y * tileSize, 0);
                    newTile.transform.rotation = Quaternion.Euler(0, 0, fullRotationMap[y, x]);

                    newTile.tag = "MapTile";
                }
            }
        }
    }
}