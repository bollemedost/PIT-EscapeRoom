using UnityEngine;

public class SplashSound : MonoBehaviour
{
    public AudioClip splashSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Afspil lyd når noget rammer "overfladen"
        audioSource.PlayOneShot(splashSound);
    }
}
// This code has been inspired by Copilot and ChatGPT.

