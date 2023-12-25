using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify EnergyPointManager that this point has been collected
            FindObjectOfType<EnergyPointManager>().CollectEnergyPoint();

            // Destroy or deactivate this EnergyPoint
            Destroy(gameObject);
        }
    }
}
