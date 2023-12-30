using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnergyPointManager energyManager = FindObjectOfType<EnergyPointManager>();
            if (energyManager != null)
            {
                energyManager.CollectEnergyPoint(gameObject);
            }
        }
    }
}
