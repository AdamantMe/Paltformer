using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardDamage : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float maxTileDistance = 1.5f; // Max distance from the tile to apply damage in the third scene
    [SerializeField] private float maxFloorDistance = 1.5f; // Max distance from the floor to apply damage in the second scene
    [SerializeField] private string hazardousFloorTag = "HazardousFloor";

    private float timeSinceLastDamage = 0f;
    private bool isOnHazardousTile = false;
    private bool isOnHazardousFloor = false;
    private HazardousFloor hazardousFloor = null;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LevelTwo")
        {
            hazardousFloor = FindObjectOfType<HazardousFloor>();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LevelTwo")
        {
            UpdateForHazardousFloor();
        }
        else if (SceneManager.GetActiveScene().name == "LevelThree")
        {
            UpdateForHazardousTiles();
        }
    }

    private void UpdateForHazardousTiles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            var hazardTile = hit.collider.GetComponent<HazardTile>();
            float distanceToTile = hit.distance;

            if (hazardTile != null && hazardTile.IsCurrentlyHazardous && distanceToTile <= maxTileDistance)
            {
                HandleHazardInteractionTile();
            }
            else
            {
                ResetHazardState();
            }
        }
        else
        {
            ResetHazardState();
        }
    }

    private void UpdateForHazardousFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag(hazardousFloorTag))
            {
                if (hit.distance < maxFloorDistance)
                {
                    if (hazardousFloor != null && hazardousFloor.IsHazardous())
                    {
                        HandleHazardInteractionFloor();
                    }
                }
                else
                {
                    isOnHazardousFloor = false;
                    timeSinceLastDamage = 0f;
                }
            }
            else
            {
                isOnHazardousFloor = false;
                timeSinceLastDamage = 0f;
            }
        }
        else
        {
            isOnHazardousFloor = false;
            timeSinceLastDamage = 0f;
        }
    }

    private void HandleHazardInteractionFloor()
    {
        if (!isOnHazardousFloor)
        {
            ApplyDamage(damagePerSecond);
            isOnHazardousFloor = true;
            timeSinceLastDamage = 0f;
        }
        else
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage >= 1f)
            {
                ApplyDamage(damagePerSecond);
                timeSinceLastDamage = 0f;
            }
        }
    }

    private void HandleHazardInteractionTile()
    {
        if (!isOnHazardousTile)
        {
            ApplyDamage(damagePerSecond);
            isOnHazardousTile = true;
            timeSinceLastDamage = 0f;
        }
        else
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage >= 1f)
            {
                ApplyDamage(damagePerSecond);
                timeSinceLastDamage = 0f;
            }
        }
    }

    private void ResetHazardState()
    {
        isOnHazardousTile = false;
        timeSinceLastDamage = 0f;
    }

    private void ApplyDamage(int damageAmount)
    {
        GameManager.Instance.ApplyDamageToPlayer(damageAmount);
    }
}
