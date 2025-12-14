using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(XRGrabInteractable))]
public class PickupSound : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        audioSource.PlayOneShot(pickupSound);
    }
}
// This code has been inspired by Copilot and ChatGPT.
