using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // A reference to the different sprites (these will need to be assigned in the Unity Inspector)
    public Sprite outsideCornerSprite; // 1 - Outside corner
    public Sprite outsideWallSprite; // 2 - Outside wall
    public Sprite insideCornerSprite; // 3 - Inside corner
    public Sprite insideWallSprite; // 4 - Inside wall
    public Sprite standardPelletSprite; // 5 - Empty space with Standard pellet
    public Sprite powerPelletSprite; // 6 - Empty space with Power pellet
    public Sprite tJunctionSprite; // 7 - T junction

    // The level layout (copied from your provided 2D array)
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

    // Tile size (used to space the tiles correctly)
    public float tileSize = 1.0f;

    void Start()
    {
        // Delete the existing level (if any) before generating the new one
        ClearExistingLevel();

        // Generate the level
        GenerateLevel();
    }

    void ClearExistingLevel()
    {
        // Find all objects with tag "MapTile" and delete them
        GameObject[] existingTiles = GameObject.FindGameObjectsWithTag("MapTile");
        foreach (GameObject tile in existingTiles)
        {
            Destroy(tile);
        }
    }

void GenerateLevel()
{
    // First, create a mirrored version of the level map for horizontal and vertical symmetry
    int originalHeight = levelMap.GetLength(0);
    int originalWidth = levelMap.GetLength(1);

    // Calculate full dimensions
    int fullHeight = originalHeight * 2;
    int fullWidth = originalWidth * 2;

    // Create a new 2D array to store the full map (with 4 quadrants)
    int[,] fullLevelMap = new int[fullHeight, fullWidth];

    // Fill the top-left quadrant (original map)
    for (int y = 0; y < originalHeight; y++)
    {
        for (int x = 0; x < originalWidth; x++)
        {
            fullLevelMap[y, x] = levelMap[y, x];
        }
    }

    // Fill the top-right quadrant (horizontally mirrored)
    for (int y = 0; y < originalHeight; y++)
    {
        for (int x = 0; x < originalWidth; x++)
        {
            fullLevelMap[y, fullWidth - 1 - x] = levelMap[y, x];  // Mirror horizontally
        }
    }

    // Fill the bottom-left quadrant (vertically mirrored)
    for (int y = 0; y < originalHeight; y++)
    {
        for (int x = 0; x < originalWidth; x++)
        {
            fullLevelMap[fullHeight - 1 - y, x] = levelMap[y, x];  // Mirror vertically
        }
    }

    // Fill the bottom-right quadrant (both horizontally and vertically mirrored)
    for (int y = 0; y < originalHeight; y++)
    {
        for (int x = 0; x < originalWidth; x++)
        {
            fullLevelMap[fullHeight - 1 - y, fullWidth - 1 - x] = levelMap[y, x];  // Mirror both
        }
    }

    // Now we iterate over the fullLevelMap and generate the tiles as before
    for (int y = 0; y < fullHeight; y++)
    {
        for (int x = 0; x < fullWidth; x++)
        {
            // Get the tile type at the current position
            int tileType = fullLevelMap[y, x];

            // Determine which sprite to use
            Sprite selectedSprite = null;
            switch (tileType)
            {
                case 0: // Empty space
                    selectedSprite = null; // No sprite for empty spaces
                    break;
                case 1: // Outside corner
                    selectedSprite = outsideCornerSprite;
                    break;
                case 2: // Outside wall
                    selectedSprite = outsideWallSprite;
                    break;
                case 3: // Inside corner
                    selectedSprite = insideCornerSprite;
                    break;
                case 4: // Inside wall
                    selectedSprite = insideWallSprite;
                    break;
                case 5: // Standard pellet
                    selectedSprite = standardPelletSprite;
                    break;
                case 6: // Power pellet
                    selectedSprite = powerPelletSprite;
                    break;
                case 7: // T junction
                    selectedSprite = tJunctionSprite;
                    break;
            }

            // Instantiate a new tile if there's a sprite to render
            if (selectedSprite != null)
            {
                // Create a new GameObject to hold the sprite
                GameObject newTile = new GameObject("Tile_" + x + "_" + y);

                // Add a SpriteRenderer component and assign the sprite to it
                SpriteRenderer spriteRenderer = newTile.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = selectedSprite;

                // Set the position of the tile using X and Y, Z fixed at 0
                newTile.transform.position = new Vector3(x * tileSize, -y * tileSize, 0);

                // Tag the new tile so it can be deleted later
                newTile.tag = "MapTile";
            }
        }
    }
}
}