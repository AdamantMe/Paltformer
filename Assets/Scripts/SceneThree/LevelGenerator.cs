using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject planePrefab;
    public GameObject playerPrefab;

    private void Start()
    {
        // Instantiate the larger plane
        GameObject plane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);

        // Create a new GameObject for the HazardManager and add the HazardManager component to it
        GameObject hazardManagerObj = new GameObject("HazardManager");
        HazardManager hazardManagerInstance = hazardManagerObj.AddComponent<HazardManager>();

        // Generate the tiles
        for (int i = 0; i < 4; i++) // 4 tiles across the width
        {
            for (int j = 0; j < 20; j++) // 20 tiles along the length
            {
                float xPosition = (i * 5f) - (5f * 1.5f);
                float zPosition = (j * 5f) - (50f - 2.5f);
                Vector3 spawnPosition = new Vector3(xPosition, 0.01f, zPosition);

                // Instantiate the tile at the calculated position
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
        Vector3 playerStartPosition = new Vector3(-7.5f, 1f, -45f);
        Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
    }
}
