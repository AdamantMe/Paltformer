using System.Collections;
using UnityEngine;

public class HazardousFloor : MonoBehaviour
{
    public Color safeColor = Color.white;
    public Color hazardColor = Color.red;
    private Renderer floorRenderer;

    public float timeToTurnHazardous = 2f;
    public float timeHazardousLasts = 3f;
    public float timeToTurnSafe = 1f;

    private bool isHazardous = false;

    void Start()
    {
        floorRenderer = GetComponent<Renderer>();
        StartCoroutine(HazardCycle());
    }

    private IEnumerator HazardCycle()
    {
        while (true)
        {
            // Wait for some time before turning hazardous
            yield return new WaitForSeconds(timeToTurnHazardous);
            floorRenderer.material.color = hazardColor;
            isHazardous = true;

            // Hazardous state lasts for a while
            yield return new WaitForSeconds(timeHazardousLasts);
            floorRenderer.material.color = safeColor;
            isHazardous = false;

            // Wait before next cycle
            yield return new WaitForSeconds(timeToTurnSafe);
        }
    }

    public bool IsHazardous()
    {
        return isHazardous;
    }
}
