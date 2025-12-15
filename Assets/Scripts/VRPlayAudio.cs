using UnityEngine;
using UnityEngine.InputSystem; 

public class VRPlayAudio : MonoBehaviour
{
    public AudioSource audioSource;  // Assign in the Inspector
    public AudioClip audioClip;      // Assign in the Inspector

    public void PlaySound()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or AudioClip!");
        }
    }
}
// This code has been inspired by Copilot and ChatGPT.

