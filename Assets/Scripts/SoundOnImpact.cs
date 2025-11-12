using UnityEngine;

public class SoundOnImpact : MonoBehaviour
{
    public AudioClip impactSound;
    public float impactThreshold = 2f; // Minimum impact speed to play sound
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check the impact force
        float impactForce = collision.relativeVelocity.magnitude;

        // Play sound only if the impact is above the threshold
        if (impactForce > impactThreshold)
        {
            audioSource.PlayOneShot(impactSound);
        }
    }
}
