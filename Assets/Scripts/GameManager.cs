using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    [Tooltip("The Y-axis height at which the player dies.")]
    [SerializeField] private Canvas canvasGameOver;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text healthText;
    public Canvas canvasIngame;
    public EnergyPointManager energyPointManager;

    public GameConfiguration GameConfig;
    public static GameManager Instance { get; private set; }

    private bool isGameBeingReset = false;
    private bool isUIVisible = true;

    private int health = 100;
    public int Health
    {
        get => health;
        private set
        {
            health = value;
            UpdateUI();
        }
    }
    public bool IsDead { get; private set; } = false;

    private float lastFallTime = -1f;
    private float fallCooldown = 2.0f;

    public PortalManager portalManager;

    public TMP_Text collectedCounterText;

    private Vector3 initialPlayerPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(canvasIngame.gameObject);
            DontDestroyOnLoad(canvasGameOver.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        initialPlayerPosition = player.transform.position;
        SetupUI();
        portalManager.ShowPortal(); // TODO REMOVE
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isGameBeingReset = false;

        if (GameConfig != null)
        {
            if (scene.name == GameConfig.SceneOne)
            {
                ReassignComponentsFirstScene();
            }
            else if (scene.name == GameConfig.SceneTwo)
            {
                ReassignComponentsSecondScene();
            }
            else
            {

            }
        }
        else
        {
            Debug.LogError("GameConfig is not set in GameManager.");
        }
    }

    private void ReassignComponentsFirstScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        donutSpawner = FindObjectOfType<DonutSpawner>();
        portalManager = FindObjectOfType<PortalManager>();

        if (donutSpawner != null)
        {
            donutSpawner.InitializeSpawning();
        }

        if (portalManager == null)
        {
            //Debug.LogError("PortalManager component not found on any GameObject in the scene.");
        }
        UpdateUI();
    }

    private void ReassignComponentsSecondScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 spawnPosition = new Vector3(0, 2, 0);
            player.transform.position = spawnPosition;
            player.transform.rotation = Quaternion.identity;
        }

        energyPointManager = FindObjectOfType<EnergyPointManager>();

        if (energyPointManager == null)
        {
            Debug.LogError("EnergyPointManager not found in the scene.");
        }
        else
        {
            UpdateUI(); // Update UI for the initial state of the second scene
        }
        ResetCamera();
    }

    private void Update()
    {
        if (player && !IsDead && player.transform.position.y <= GameConfig.DeathBorder)
        {
            if (Time.time - lastFallTime > fallCooldown)
            {
                PlayerFell();
                lastFallTime = Time.time;
            }
        }

        // Listen for the Escape key to toggle UI visibility
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUIVisibility();
        }
    }
    private void ResetCamera()
    {
        if (player != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.transform.position = player.transform.position + new Vector3(0, 1.8f, 0); // Adjust 1.8f to suit player height
                mainCamera.transform.rotation = Quaternion.identity;
            }

            var fpsController = FindObjectOfType<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            if (fpsController != null)
            {
                // Toggle the enabled state of the component to reset it
                fpsController.enabled = false;
                fpsController.enabled = true;
            }
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.angularVelocity = Vector3.zero;
            }
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
                //characterController.SimpleMove(Vector3.zero);
                //player.transform.position = initialPlayerPosition;
                characterController.enabled = true;
            }
        }
    }

    private void PlayerFell()
    {
        Health -= GameConfig.HealthDeductionOnFall;

        if (Health > 0)
        {
            RespawnPlayer();
        }
        else
        {
            PlayerDied();
        }
    }

    private void RespawnPlayer()
    {
        if (player != null)
        {
            // Get the FirstPersonController script
            var fpsController = player.GetComponent<FirstPersonController>();

            // Disable the character controller
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            // Reset player position
            player.transform.position = initialPlayerPosition;

            // Re-enable the character controller
            if (characterController != null)
            {
                characterController.enabled = true;
            }

            // Reset the FirstPersonController if it exists
            if (fpsController != null)
            {
                fpsController.enabled = false;
                fpsController.enabled = true;
            }
        }
    }

    private void PlayerDied()
    {
        IsDead = true;
        Health = 0;
        ToggleUI(false);
    }

    #region UI

    private void SetupUI()
    {
        if (canvasGameOver != null)
        {
            canvasGameOver.enabled = false;
        }
        if (canvasIngame != null)
        {
            canvasIngame.enabled = true;
        }
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (healthText != null)
        {
            healthText.text = $"Health {Health}";
        }

        if (currentScene.name == GameConfig.SceneOne)
        {
            if (collectedCounterText != null && donutSpawner != null)
            {
                collectedCounterText.text = $"Collected: {DonutsCollected}/{donutSpawner.GetDonutsSpawned()}";
            }
        }
        else if (currentScene.name == GameConfig.SceneTwo)
        {
            if (collectedCounterText != null && energyPointManager != null)
            {
                int energyPointsCollected = energyPointManager.GetEnergyPointsCollected();
                collectedCounterText.text = $"Collected: {energyPointsCollected}/{GameConfig.EnergyPointsToSpawn}";
            }
        }
    }

    private void ToggleUI(bool active)
    {
        if (canvasIngame != null)
        {
            canvasIngame.enabled = active;
        }
        if (canvasGameOver != null)
        {
            canvasGameOver.enabled = !active;
            if (!active)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void ToggleUIVisibility()
    {
        isUIVisible = !isUIVisible; // Toggle the state
        canvasIngame.enabled = isUIVisible;
        canvasGameOver.enabled = !isUIVisible;

        // Toggle cursor visibility and lock state
        Cursor.visible = isUIVisible;
        Cursor.lockState = isUIVisible ? CursorLockMode.None : CursorLockMode.Locked;

        // Pause the game if the UI is visible
        Time.timeScale = isUIVisible ? 0f : 1f;

        var fpsController = player.GetComponent<FirstPersonController>();
        if (fpsController != null)
        {
            fpsController.enabled = !isUIVisible;
        }
    }

    #endregion UI

    public void ResetGame()
    {
        isGameBeingReset = true; // Set the flag to true when resetting the game

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        Health = 100;
        IsDead = false;
        ToggleUI(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RespawnPlayer();

        // Only reset donutsCollected if the game is being reset
        if (isGameBeingReset)
        {
            DonutsCollected = 0;
            if (donutSpawner != null)
            {
                donutSpawner.InitializeSpawning();
            }
        }
        UpdateUI();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region DONUTS
    public DonutSpawner donutSpawner;

    private int donutsCollected;
    public int DonutsCollected
    {
        get => donutsCollected;
        set
        {
            donutsCollected = value;
            UpdateUI();
        }
    }


    public void ShowPrompt()
    {
        promptText?.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        promptText?.gameObject.SetActive(false);
    }

    public bool PromptIsActive()
    {
        return promptText != null && promptText.gameObject.activeSelf;
    }

    public void IncrementDonutsCollected()
    {
        DonutsCollected++;

        if (DonutsCollected >= 0) // TODO Change back to "DonutsCollected == donutSpawner.GetDonutsSpawned()"
        {
            portalManager.ShowPortal();
        }
    }

    #endregion
}
