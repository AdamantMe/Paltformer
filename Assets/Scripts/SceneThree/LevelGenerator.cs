using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameConfiguration gameConfig;
    public GameObject tilePrefab;
    public GameObject planePrefab;
    public GameObject playerPrefab;
    public GameObject energyPointPrefab;
    public int numberOfEnergyPoints = 10;

    private List<GameObject> allTiles = new List<GameObject>();

    private void Start()
    {
        GeneratePlaneAndTiles();
        SpawnPlayer();
        SpawnEnergyPoints();
    }

    private void GeneratePlaneAndTiles()
    {
        GameObject plane = Instantiate(planePrefab, Vector3.zero, Quaternion.identity);

        GameObject hazardManagerObj = new GameObject("HazardManager");
        HazardManager hazardManagerInstance = hazardManagerObj.AddComponent<HazardManager>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                float xPosition = (i * 5f) - (5f * 1.5f);
                float zPosition = (j * 5f) - (200f - 2.5f);
                Vector3 spawnPosition = new Vector3(xPosition, 0.01f, zPosition);

                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                tile.transform.localScale = new Vector3(0.5f, 1f, 0.5f);

                // Add the HazardTile component to each tile
                HazardTile hazardTile = tile.AddComponent<HazardTile>();
                hazardManagerInstance.AddTile(hazardTile);

                allTiles.Add(tile);
            }
        }

        // Initialize the HazardManager to start the hazard effect cycle
        hazardManagerInstance.Initialize();
    }

    private void SpawnPlayer()
    {
        Vector3 playerStartPosition = new Vector3(-7.5f, 1f, -195f);
        Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
    }

    private void SpawnEnergyPoints()
    {
        HashSet<int> occupiedTiles = new HashSet<int>();

        for (int i = 0; i < gameConfig.MaximumCollectablesOnLevelThree; i++)
        {
            int randomTileIndex;
            do
            {
                randomTileIndex = Random.Range(0, allTiles.Count);
            }
            while (occupiedTiles.Contains(randomTileIndex));

            GameObject chosenTile = allTiles[randomTileIndex];
            Vector3 spawnPosition = chosenTile.transform.position + Vector3.up * 2; 
            Instantiate(energyPointPrefab, spawnPosition, Quaternion.identity);

            occupiedTiles.Add(randomTileIndex); 
        }
    }

}
