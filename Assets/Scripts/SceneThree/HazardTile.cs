using System.Collections;
using UnityEngine;

public class HazardTile : MonoBehaviour
{
    public GameConfiguration gameConfig;
    public Color safeColor = Color.white;
    public Color hazardColor = Color.red;
    private Renderer renderer;

    private float timeToTurnRed = 1f;
    private float timeRedLasts = 1f;
    private float timeToTurnWhite = 0.5f;
    public bool IsCurrentlyHazardous { get; private set; }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void StartHazardEffect()
    {
        StartCoroutine(HazardEffect());
    }

    private IEnumerator HazardEffect()
    {
        // Gradually turn red
        float elapsedTime = 0f;
        while (elapsedTime < timeToTurnRed)
        {
            renderer.material.color = Color.Lerp(safeColor, hazardColor, elapsedTime / timeToTurnRed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = hazardColor;

        // Set tile as hazardous only after it turns completely red
        IsCurrentlyHazardous = true;

        // Stay red for a while
        yield return new WaitForSeconds(timeRedLasts);

        // Reset hazardous state before turning back
        IsCurrentlyHazardous = false;

        // Gradually turn back to white
        elapsedTime = 0f;
        while (elapsedTime < timeToTurnWhite)
        {
            renderer.material.color = Color.Lerp(hazardColor, safeColor, elapsedTime / timeToTurnWhite);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color = safeColor;
    }

}
