﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnergyPointManager : MonoBehaviour
{
    public GameConfiguration gameConfig;
    public GameObject energyPointPrefab;
    public List<Transform> spawnPointsSceneOne;
    private List<GameObject> spawnedEnergyPoints = new List<GameObject>();

    private int energyPointsCollected;
    public int EnergyPointsCollected
    {
        get => energyPointsCollected;
        private set
        {
            energyPointsCollected = value;
            GameManager.Instance.UpdateUI();
        }
    }

    public void InitializeSpawning()
    {
        ClearEnergyPoints();
        EnergyPointsCollected = 0;

        if (SceneManager.GetActiveScene().name == GameManager.Instance.GameConfig.SceneOne)
        {
            SpawnEnergyPointsSceneOne();
        }
        else if (SceneManager.GetActiveScene().name == GameManager.Instance.GameConfig.SceneTwo)
        {
            SpawnEnergyPointSceneTwo();
        }
        else if (SceneManager.GetActiveScene().name == GameManager.Instance.GameConfig.SceneTwo)
        {
            SpawnEnergyPointSceneThree();
        }
    }

    private void ClearEnergyPoints()
    {
        foreach (GameObject energyPoint in spawnedEnergyPoints)
        {
            if (energyPoint != null)
            {
                Destroy(energyPoint);
            }
        }
        spawnedEnergyPoints.Clear();
    }
    
    public void CollectEnergyPoint(GameObject energyPoint)
    {
        EnergyPointsCollected++;
        energyPoint.SetActive(false); // Deactivate the collected energy point
        
        // Check if all energy points have been collected
        //if (EnergyPointsCollected >= gameConfig.MaximumCollectablesOnLevelOne) // TODO
        {
            GameManager.Instance.HandleEnergyPointCollection();
        }
    }

    private void SpawnEnergyPointsSceneOne()
    {
        HashSet<Transform> chosenIslands = new HashSet<Transform>();

        while (spawnedEnergyPoints.Count < gameConfig.MaximumCollectablesOnLevelOne && spawnPointsSceneOne.Count > 0)
        {
            Transform selectedIsland = spawnPointsSceneOne[Random.Range(0, spawnPointsSceneOne.Count)];

            if (IsFarEnoughFromOthers(selectedIsland, chosenIslands))
            {
                GameObject energyPoint = Instantiate(energyPointPrefab, selectedIsland.position + Vector3.up * 2, Quaternion.identity);
                spawnedEnergyPoints.Add(energyPoint);
                chosenIslands.Add(selectedIsland);
            }
        }
    }

    private void SpawnEnergyPointSceneTwo()
    {
        Vector3 spawnArea = gameConfig.CubeAreaSize * gameConfig.EnergyPointSpawnAreaFactor;
        Vector3 spawnPosition = GetRandomSpawnPosition(spawnArea);

        Instantiate(energyPointPrefab, spawnPosition, Quaternion.identity);
    }
    

    private void SpawnEnergyPointSceneThree()
    {
    }

    private Vector3 GetRandomSpawnPosition(Vector3 spawnArea)
    {
        // Implement your logic using spawnArea
        return new Vector3(Random.Range(-spawnArea.x / 2, spawnArea.x / 2), Random.Range(2f, (float)(spawnArea.y * 0.5)), Random.Range(-spawnArea.z / 2, spawnArea.z / 2));
    }

    public int GetEnergyPointsCollected()
    {
        return EnergyPointsCollected;
    }

    private bool IsFarEnoughFromOthers(Transform island, HashSet<Transform> otherIslands)
    {
        foreach (Transform other in otherIslands)
        {
            if (Vector3.Distance(island.position, other.position) < gameConfig.MinimumDistanceBetweenEnergyPoints)
            {
                return false; // Not far enough from another island
            }
        }
        return true; // Far enough from all other islands
    }
}