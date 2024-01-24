using System.Collections;
using UnityEngine;

public class EnergyPoint : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!audioSource.isActiveAndEnabled )
                audioSource.enabled = true;
            audioSource.Play();
            EnergyPointManager energyManager = FindObjectOfType<EnergyPointManager>();
            if (energyManager != null)
            {
                energyManager.CollectEnergyPoint(gameObject);
                StartCoroutine(WaitAndDestroy(audioSource.clip.length));
            }
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        GetComponent<Collider>().enabled = false;

        // Wait for the sound to finish
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }
}
