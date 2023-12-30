using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject planePrefab;
    public GameObject playerPrefab;

    private void Start()
    {
        GameObject plane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);

        // Create a new GameObject for the HazardManager and add the HazardManager component to it
        GameObject hazardManagerObj = new GameObject("HazardManager");
        HazardManager hazardManagerInstance = hazardManagerObj.AddComponent<HazardManager>();

        // Generate the tiles
        int tilesPerRow = 4;
        int numberOfRows = 80; 

        for (int i = 0; i < tilesPerRow; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                float xPosition = (i * 5f) - (5f * 1.5f);
                float zPosition = (j * 5f) - (numberOfRows * 5f / 2) + 2.5f;

                Vector3 spawnPosition = new Vector3(xPosition, 0.01f, zPosition);
                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                tile.transform.localScale = new Vector3(0.5f, 1f, 0.5f);

                // Dynamically attach the HazardTile script
                HazardTile hazardTile = tile.AddComponent<HazardTile>();
                hazardManagerInstance.AddTile(hazardTile);
            }
        }

        // Initialize the HazardManager to start the hazard effect cycle
        hazardManagerInstance.Initialize();

        // Spawn the player
        Vector3 playerStartPosition = new Vector3(0, 1f, -200); // Position player at the start of the plane
        GameObject newPlayer = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);

        // Update GameManager's player reference
        GameManager.Instance.SetPlayer(newPlayer);
    }
}
