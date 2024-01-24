using UnityEngine;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    private EventSystem eventSystem;

    public void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    public void ReloadGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();

            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(null);
            }
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quit game fired.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit the game in the editor
#else
        Application.Quit(); // Quit the game in a build
#endif
    }

}
