using UnityEngine;
using System.Collections;
using System.Linq;

namespace Assets.Scripts.SceneTwo
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameConfiguration gameConfig;
        public GameObject cubePrefab;
        public float spacing = 0.2f; // Additional spacing to ensure cubes don't touch

        void Start()
        {
            GenerateLevel();
        }

        void GenerateLevel()
        {
            Vector3 areaSize = gameConfig.CubeAreaSize;
            Vector3 playerPosition = new Vector3(0, 2, 0);
            float safeRadius = 10.0f; // Radius around the player to keep free of cubes

            int attempts = 0;
            int maxAttempts = gameConfig.CubesToSpawn * 10; // Prevent infinite loop

            for (int i = 0; i < gameConfig.CubesToSpawn && attempts < maxAttempts;)
            {
                float randomY = Mathf.Pow(Random.value, 2); // Squared distribution
                int addDeviation = Random.Range(0, 100);
                float cubeSpawnY = addDeviation < 25 ? Random.Range(0f, 3) : Random.Range(0f, areaSize.y);

                Vector3 randomPosition = new Vector3(
                    Random.Range(-areaSize.x, areaSize.x),
                    cubeSpawnY,
                    Random.Range(-areaSize.z, areaSize.z)
                );

                if (Vector3.Distance(randomPosition, playerPosition) < safeRadius)
                {
                    attempts++;
                    continue; // Skip this iteration if too close to the player
                }

                Vector3 randomScale = GetRandomScale(Random.Range(0, 100));
                Collider[] hitColliders = Physics.OverlapBox(randomPosition, (randomScale / 2) + Vector3.one * spacing);

                if (hitColliders.Length == 0 || hitColliders.All(collider => collider.CompareTag("PlaneTag")))
                {
                    GameObject cube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
                    cube.transform.localScale = randomScale;
                    i++; // Only increment if a cube was successfully placed
                }
                else
                {
                    attempts++;
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Max attempts reached, could not place all cubes without overlap.");
            }
        }

        Vector3 GetRandomScale(int randomDeviation)
        {
            if (randomDeviation > 95)
                return Vector3.one * Random.Range(3f, 12f);
            else if (randomDeviation > 90)
                return Vector3.one * Random.Range(1f, 7f);
            else if (randomDeviation < 20)
                return Vector3.one * Random.Range(1f, 4f);
            else
                return Vector3.one * Random.Range(0.5f, 2f);
        }
    }
}