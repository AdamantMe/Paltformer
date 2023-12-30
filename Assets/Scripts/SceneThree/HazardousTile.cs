using UnityEngine;

public class HazardousTile : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ApplyDamageToPlayer(1);
        }
    }
}
