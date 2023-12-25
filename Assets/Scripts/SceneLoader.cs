using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void ReloadGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
    }
}
