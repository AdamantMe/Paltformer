using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas canvasGameOver;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private Button retryButton;

    public Canvas canvasIngame;
    public EnergyPointManager energyPointManager;
    public GameConfiguration GameConfig;
    public static GameManager Instance { get; private set; }

    private bool isUIVisible = true;
    private float startTime;

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
            DontDestroyOnLoad(canvasIngame.gameObject);
            DontDestroyOnLoad(canvasGameOver.gameObject);

            // Find and persist the EnergyPointManager if it's in the scene
            EnergyPointManager existingManager = FindObjectOfType<EnergyPointManager>();
            if (existingManager != null)
            {
                energyPointManager = existingManager;
                DontDestroyOnLoad(energyPointManager.gameObject);
            }

            if (portalManager != null && portalManager.gameObject != gameObject)
            {
                DontDestroyOnLoad(portalManager.gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        startTime = Time.time;
        txtScore.gameObject.SetActive(false);
        initialPlayerPosition = player.transform.position;
        InitializeUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
            else if (scene.name == GameConfig.SceneThree)
            {
                ReassignComponentsThirdScene();
            }
            energyPointManager.InitializeSpawning();
        }
        else
        {
            Debug.LogError("GameConfig is not set in GameManager.");
        }
        UpdateUI();
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

    public void PlayerDied()
    {
        IsDead = true;
        Health = 0;
        ShowFinalScreen(false);
    }

    public void PlayerFinishedGame()
    {
        ShowFinalScreen(true);
    }

    #region UI

    private void InitializeUI()
    {
        // Set the game to be active (not paused) and hide the cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set the initial visibility of UI elements
        if (canvasGameOver != null)
        {
            canvasGameOver.enabled = false; // Ensure game over menu is not visible initially
        }
        if (canvasIngame != null)
        {
            canvasIngame.enabled = true; // Ensure in-game UI is visible initially
        }
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        // Set UI state to not visible
        isUIVisible = false;
    }

    public void UpdateUI()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (healthText != null)
        {
            healthText.text = $"Health {Health}";
        }

        if (energyPointManager != null && collectedCounterText != null)
        {
            int energyPointsCollected = energyPointManager.GetEnergyPointsCollected();

            if (currentScene.name == GameConfig.SceneOne)
            {
                collectedCounterText.text = $"Collected: {energyPointsCollected}/{GameConfig.EnergyPointsToSpawn}";
            }
            else if (currentScene.name == GameConfig.SceneTwo)
            {
                collectedCounterText.text = $"Collected: {energyPointsCollected}/{GameConfig.EnergyPointsToSpawn}";
            }
            else if (currentScene.name == GameConfig.SceneThree)
            {
                collectedCounterText.text = $"Collected: {energyPointsCollected}/{GameConfig.EnergyPointsToSpawn}";
                if (energyPointsCollected >= GameConfig.EnergyPointsToSpawn)
                {
                    ShowFinalScreen(true);
                }
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

        // Toggle UI elements and cursor visibility based on the isUIVisible flag
        if (canvasIngame != null)
        {
            canvasIngame.enabled = !isUIVisible;
        }
        if (canvasGameOver != null)
        {
            canvasGameOver.enabled = isUIVisible;
        }

        Cursor.lockState = isUIVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isUIVisible;
        Time.timeScale = isUIVisible ? 0f : 1f;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // Enable or disable the FPS controller based on the UI visibility
        if (player != null)
        {
            var fpsController = player.GetComponent<FirstPersonController>();
            if (fpsController != null)
            {
                fpsController.enabled = !isUIVisible;
            }
        }
    }

    #endregion UI

    public void ResetGame()
    {
        // Reset the health to full
        Health = GameConfig.PlayerMaxHealth;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        InitializeUI();
        IsDead = false;

        // Ensure the UI is set correctly
        ToggleUI(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region First Scene

    private void ReassignComponentsFirstScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        energyPointManager = FindObjectOfType<EnergyPointManager>();
    }

    #endregion First Scene

    #region Second Scene

    public void HandleEnergyPointCollection()
    {
        // Reflect the UI changes in energy points collected
        UpdateUI();

        // For scene one spawn the portal in the middle of the islands
        if (SceneManager.GetActiveScene().name == GameConfig.SceneOne)
        {
            Vector3 portalSpawnPosition = new Vector3(33, 1, -11);
            portalManager.SpawnPortalAt(portalSpawnPosition);
        }
        else
        {
            portalManager.SpawnPortalAt(initialPlayerPosition);
        }
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

        if (energyPointManager != null)
        {
            energyPointManager.ToggleIslandsVisibility(false); // Deactivate islands in other scenes
        }
    }


    #endregion Second Scene

    #region Third Scene

    private void ReassignComponentsThirdScene()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        energyPointManager = FindObjectOfType<EnergyPointManager>();
        initialPlayerPosition = new Vector3(-7.5f, 1f, -195f);
    }

    public void ApplyDamageToPlayer(float damage)
    {
        Health -= (int)damage;
        if (Health <= 0)
        {
            PlayerDied();
        }
        UpdateUI();
    }

    #endregion Third Scene

    #region Final Score

    private void ShowFinalScreen(bool completedGame)
    {
        if (canvasGameOver != null)
        {
            canvasGameOver.enabled = true;
            canvasIngame.enabled = false;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (completedGame || IsDead)
            {
                UpdateScoreUI(completedGame);
                txtScore.gameObject.SetActive(true);
            }
            else
            {
                txtScore.gameObject.SetActive(false);
            }

            if (completedGame)
            {
                retryButton.interactable = false;
            }
            else
            {
                retryButton.interactable = true;
            }
        }
    }

    private void UpdateScoreUI(bool completedGame)
    {
        if (completedGame || IsDead)
        {
            float timeTaken = Time.time - startTime;
            float score = CalculateScore(Health, timeTaken);

            string scoreDetails = $"Health left: {Health}\n\n" +
                                  $"Time taken: {timeTaken:F2} seconds\n\n" +
                                  $"Total Score: {score}/10";

            txtScore.text = scoreDetails;
        }
    }

    private float CalculateScore(int health, float time)
    {
        var levelTimeLimit = GameConfig.LevelTimeLimit;
        // Full score for completing under 5 minutes and full health
        if (time <= levelTimeLimit && health == 100)
        {
            return 10.0f;
        }
        // Decrease score based on health and time
        float timeScore = (levelTimeLimit - time) / levelTimeLimit * 5.0f;
        float healthScore = (float)health / 100 * 5.0f;
        float totalScore = timeScore + healthScore;

        // Round (up) the score to one decimal place
        return (float)Math.Round(totalScore, 1, MidpointRounding.AwayFromZero);
    }

    #endregion Final Score
}