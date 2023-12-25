using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    // Static variable to track the existence of the object
    private static bool objectExists = false;

    void Awake()
    {
        if (!objectExists)
        {
            // Object does not exist yet, set the flag and don't destroy it
            DontDestroyOnLoad(gameObject);
            objectExists = true;
        }
        else
        {
            // Object already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
}
