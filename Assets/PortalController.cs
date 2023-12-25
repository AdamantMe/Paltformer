using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public string secondSceneName = "LevelTwo";

    private void Start()
    {
        gameObject.SetActive(false); 
    }

    public void ShowPortal()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(secondSceneName);
        }
    }
}
