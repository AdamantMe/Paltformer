using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    private List<HazardTile> allTiles = new List<HazardTile>();

    public void AddTile(HazardTile tile)
    {
        allTiles.Add(tile);
    }

    public void Initialize()
    {
        // Start the hazard effect cycle
        InvokeRepeating(nameof(ActivateRandomTiles), 1f, 3f);
    }

    private void ActivateRandomTiles()
    {
        // Randomly choose tiles to activate
        for (int i = 0; i < allTiles.Count; i += 4) // Iterate over each row
        {
            int tilesToActivate = Random.Range(1, 5); // At least 1, at most 4 tiles per row
            HashSet<int> activatedTilesIndexes = new HashSet<int>();

            while (activatedTilesIndexes.Count < tilesToActivate)
            {
                int tileIndexWithinRow = Random.Range(0, 4); // Random index within the row
                int absoluteTileIndex = i + tileIndexWithinRow; // Calculate absolute index

                if (!activatedTilesIndexes.Contains(absoluteTileIndex) && absoluteTileIndex < allTiles.Count)
                {
                    allTiles[absoluteTileIndex].StartHazardEffect();
                    activatedTilesIndexes.Add(absoluteTileIndex);
                }
            }
        }
    }
}
