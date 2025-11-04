using UnityEngine;

public class SoundOnImpact : MonoBehaviour
{
    public AudioClip impactSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Afspil lyd hver gang objektet rammer noget
        audioSource.PlayOneShot(impactSound);
    }
}

