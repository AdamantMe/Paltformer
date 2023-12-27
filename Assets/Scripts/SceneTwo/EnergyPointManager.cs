using UnityEngine;

public class EnergyPointManager : MonoBehaviour
{
    public GameConfiguration gameConfig;
    public GameObject energyPointPrefab;
    
    private int energyPointsCollected = 0;
    public int EnergyPointsCollected 
    {
        get
        {
            return energyPointsCollected;
        }
        set
        {
            energyPointsCollected = value;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateUI();
            }
        }
    }

    private void Start()
    {
        // Spawn the first EnergyPoint
        SpawnEnergyPoint();
    }

    public void CollectEnergyPoint()
    {
        EnergyPointsCollected++;

        if (EnergyPointsCollected < gameConfig.EnergyPointsToSpawn)
        {
            // Spawn next EnergyPoint
            SpawnEnergyPoint();
        }
        else
        {
            // Delegate the task to GameManager
            GameManager.Instance.HandleEnergyPointCollection();
        }
    }

    private void SpawnEnergyPoint()
    {
        Vector3 spawnArea = gameConfig.CubeAreaSize * gameConfig.EnergyPointSpawnAreaFactor;
        Vector3 spawnPosition = GetRandomSpawnPosition(spawnArea);

        Instantiate(energyPointPrefab, spawnPosition, Quaternion.identity);
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
}
