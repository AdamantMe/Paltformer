using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portal;

    public string secondSceneName = "LevelTwo";

    private void Start()
    {
        //portal.SetActive(false);
    }

    public void ShowPortal()
    {
        portal.SetActive(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(secondSceneName);
        }
    }
}
