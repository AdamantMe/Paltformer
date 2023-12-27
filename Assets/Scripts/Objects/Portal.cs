using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the PortalManager that the player has entered the portal
            PortalManager.Instance.PlayerEnteredPortal(this);
        }
    }
}
