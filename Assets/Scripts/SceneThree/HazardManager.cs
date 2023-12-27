using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public GameObject tileParent;
    private List<HazardTile> allTiles = new List<HazardTile>();

    public void AddTile(HazardTile tile)
    {
        allTiles.Add(tile);
    }

    public void Initialize()
    {
        // Start the hazard effect cycle
        InvokeRepeating(nameof(ActivateRandomTiles), 2f, 6f);
    }

    private void ActivateRandomTiles()
    {
        // Randomly choose tiles to activate
        for (int i = 0; i < allTiles.Count; i += 4)
        {
            int tilesToActivate = Random.Range(1, 4); // At least 1, at most 3
            HashSet<int> activatedTilesIndexes = new HashSet<int>();
            while (activatedTilesIndexes.Count < tilesToActivate)
            {
                int tileIndex = Random.Range(i, i + 4);
                if (!activatedTilesIndexes.Contains(tileIndex))
                {
                    allTiles[tileIndex].StartHazardEffect();
                    activatedTilesIndexes.Add(tileIndex);
                }
            }
        }
    }
}
