using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class SortingHat : MonoBehaviour
{
    public AudioClip[] houseSounds; // 0 = Gryffindor, 1 = Hufflepuff, etc.
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;
    private bool hasBeenGrabbed = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Listen for grab event
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasBeenGrabbed) return;

        hasBeenGrabbed = true;

        int index = Random.Range(0, houseSounds.Length);
        audioSource.PlayOneShot(houseSounds[index]);
    }
}
// This code has been inspired by Copilot and ChatGPT.

