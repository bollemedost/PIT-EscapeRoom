using UnityEngine;

public class StoneTarget : MonoBehaviour
{
    [Header("Required Stone")]
    public GameObject magicStone; // Assign the magic stone here

    [Header("Placement Settings")]
    public Transform placementPoint; // Optional: position where stone should snap

    [Header("Audio/Visual Feedback")]
    public ParticleSystem onPlacedParticles;
    public AudioSource audioSource;
    public AudioClip placeSound;

    private bool stonePlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (stonePlaced) return;

        if (other.gameObject == magicStone)
        {
            stonePlaced = true;
            SnapStone();
            PlayEffects();
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.StonePlaced);
            Debug.Log("✅ Magic stone placed on target!");
        }
    }

    private void SnapStone()
    {
        if (placementPoint != null)
        {
            magicStone.transform.position = placementPoint.position;
            magicStone.transform.rotation = placementPoint.rotation;
        }

        Rigidbody rb = magicStone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void PlayEffects()
    {
        if (audioSource != null && placeSound != null)
            audioSource.PlayOneShot(placeSound);

        if (onPlacedParticles != null)
            onPlacedParticles.Play();
    }
}
