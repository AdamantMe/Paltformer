using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance { get; private set; }

    [SerializeField] private GameObject portalPrefab;
    private GameObject currentPortalInstance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of PortalManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SpawnPortalAt(Vector3 spawnPosition)
    {
        // Check if a portal already exists, and if so, destroy it.
        if (currentPortalInstance != null)
        {
            Destroy(currentPortalInstance);
        }

        // Instantiate the portal prefab at the given position
        currentPortalInstance = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
        currentPortalInstance.SetActive(true);
    }

    public void PlayerEnteredPortal(Portal portal)
    {
        // Decide which scene to load based on the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        string nextSceneName = GetNextSceneName(currentScene.name);

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private string GetNextSceneName(string currentSceneName)
    {
        if (currentSceneName == GameManager.Instance.GameConfig.SceneOne)
        {
            return GameManager.Instance.GameConfig.SceneTwo;
        }
        else if (currentSceneName == GameManager.Instance.GameConfig.SceneTwo)
        {
            return GameManager.Instance.GameConfig.SceneThree;
        }
        return null;
    }

}
