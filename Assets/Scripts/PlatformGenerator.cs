using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject[] platformPrefabs; // Array to hold your platform prefabs.
    public int numberOfPlatforms = 7;
    public float minJumpDistance = 5f; // Minimum distance the player should be able to jump.
    public float maxJumpDistance = 10f; // Maximum distance the player should be able to jump.

    private Vector3 platformScale = new Vector3(100f, 100f, 100f); // The scale set for each island.

    void Start()
    {
        if (platformPrefabs.Length == 0)
        {
            Debug.LogError("Platform prefabs not assigned!");
            return;
        }

        // Assuming player is already placed in the scene on a platform.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        Vector3 playerPosition = player.transform.position;

        // Start spawning platforms.
        SpawnPlatforms(playerPosition);
    }

    void SpawnPlatforms(Vector3 playerPosition)
    {
        Vector3 lastPlatformPosition = playerPosition;

        // Adjust the position to place the first platform below the player.
        lastPlatformPosition.y -= 1f; // A slight offset from the player's feet.

        // Instantiate the first platform directly below the player.
        GameObject firstPlatform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], lastPlatformPosition, Quaternion.Euler(0, 0, 0));
        firstPlatform.transform.localScale = platformScale;

        // Correct the orientation of the platform in case it's spawned on its side.
        firstPlatform.transform.rotation = Quaternion.identity;

        Vector3 lastPlatformSize = firstPlatform.GetComponent<Renderer>().bounds.size;

        for (int i = 1; i < numberOfPlatforms; i++) // We already spawned the first platform.
        {
            Vector3 positionOffset = CalculatePositionOffset(lastPlatformSize);

            // Calculate the next platform's position.
            Vector3 nextPlatformPosition = lastPlatformPosition + positionOffset;

            // Instantiate the next platform.
            GameObject platformInstance = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], nextPlatformPosition, Quaternion.Euler(0, 0, 0));
            platformInstance.transform.localScale = platformScale;

            // Correct the orientation of the platform in case it's spawned on its side.
            platformInstance.transform.rotation = Quaternion.identity;

            // Update lastPlatformPosition and lastPlatformSize for the next iteration.
            lastPlatformPosition = platformInstance.transform.position;
            lastPlatformSize = platformInstance.GetComponent<Renderer>().bounds.size;
        }
    }

    Vector3 CalculatePositionOffset(Vector3 lastPlatformSize)
    {
        // Determine a random distance within the player's jump range, considering the size of the last platform.
        float distanceToEdge = lastPlatformSize.x / 2;
        float distance = Random.Range(minJumpDistance, maxJumpDistance);

        // Determine a random direction to place the next island, only on the XZ plane.
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        direction.Normalize();

        return direction * distance;
    }
}
