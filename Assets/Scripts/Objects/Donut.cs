using UnityEngine;

public class Donut : MonoBehaviour
{
    private bool isCollected = false;
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (!isCollected && isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            GameManager.Instance.ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            GameManager.Instance.HidePrompt();
        }
    }
    private void Collect()
    {
        isCollected = true;
        GameManager.Instance.IncrementDonutsCollected();
        GameManager.Instance.HidePrompt(); // Hide the prompt after collecting the donut
        gameObject.SetActive(false); // Deactivate the donut GameObject
    }


}
