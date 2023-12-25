using System.Collections.Generic;
using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    public GameObject donutPrefab; // Assign in inspector, the prefab you want to spawn
    public List<Transform> allIslands; // Assign in inspector, all the island transforms
    public int donutsToSpawn = 10; // Number of donuts you want to spawn
    public float minimumDistance = 17f; // Minimum distance between donuts

    private List<GameObject> spawnedDonuts = new List<GameObject>(); // Keeps track of all spawned donuts

    public void InitializeSpawning()
    {
        ClearDonuts();
        SpawnDonuts();
    }

    private void ClearDonuts()
    {
        foreach (GameObject donut in spawnedDonuts)
        {
            if (donut != null)
            {
                Destroy(donut);
            }
        }
        spawnedDonuts.Clear();
    }

    private void SpawnDonuts()
    {
        List<Transform> availableIslands = new List<Transform>(allIslands);
        List<Transform> chosenIslands = new List<Transform>();

        while (donutsToSpawn > 0 && availableIslands.Count > 0)
        {
            // Randomly select an island
            Transform chosenIsland = availableIslands[Random.Range(0, availableIslands.Count)];

            // Check if it's far enough from other chosen islands
            if (IsFarEnoughFromOthers(chosenIsland, chosenIslands))
            {
                // Instantiate donut above the island
                Vector3 spawnPosition = chosenIsland.position + Vector3.up * 2; // Adjust the Vector3.up * 2 as needed to be above the island
                GameObject donutObject = Instantiate(donutPrefab, spawnPosition, Quaternion.identity);
                spawnedDonuts.Add(donutObject);

                // Add to the chosen list and remove from available list
                chosenIslands.Add(chosenIsland);
                availableIslands.Remove(chosenIsland);

                donutsToSpawn--;
            }
            else
            {
                // Remove from available list if not far enough
                availableIslands.Remove(chosenIsland);
            }
        }
    }

    private bool IsFarEnoughFromOthers(Transform island, List<Transform> otherIslands)
    {
        foreach (Transform other in otherIslands)
        {
            if (Vector3.Distance(island.position, other.position) < minimumDistance)
            {
                return false; // Not far enough from another island
            }
        }
        return true; // Far enough from all other islands
    }

    public int GetDonutsSpawned()
    {
        return spawnedDonuts.Count;
    }
}
