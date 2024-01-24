using UnityEngine;
using System.Collections.Generic;

public class EnergyPointSpawner : MonoBehaviour
{
    public GameObject energyPointPrefab;
    public int numberOfPointsToSpawn = 10;
    public float minimumDistanceBetweenPoints = 5f; // Minimum distance between energy points
    public Vector2 planeSize = new Vector2(40, 40); // Size of the plane

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        SpawnEnergyPoints();
    }

    private void SpawnEnergyPoints()
    {
        for (int i = 0; i < numberOfPointsToSpawn; i++)
        {
            Vector3 potentialPosition;
            bool positionFound;

            do
            {
                positionFound = true;
                potentialPosition = new Vector3(
                    Random.Range(-planeSize.x / 2, planeSize.x / 2),
                    0.5f, // Height at which energy points spawn
                    Random.Range(-planeSize.y / 2, planeSize.y / 2)
                );

                foreach (Vector3 otherPosition in spawnedPositions)
                {
                    if (Vector3.Distance(potentialPosition, otherPosition) < minimumDistanceBetweenPoints)
                    {
                        positionFound = false;
                        break;
                    }
                }
            }
            while (!positionFound);

            GameObject newEnergyPoint = Instantiate(energyPointPrefab, potentialPosition, Quaternion.identity);
            spawnedPositions.Add(potentialPosition);
        }
    }
}
