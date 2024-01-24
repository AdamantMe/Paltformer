using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    private static bool objectExists = false;

    void Awake()
    {
        if (!objectExists)
        {
            DontDestroyOnLoad(gameObject);
            objectExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
