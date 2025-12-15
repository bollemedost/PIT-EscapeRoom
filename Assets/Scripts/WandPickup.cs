using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WandPickup : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;      // Source to play the sound
    public AudioClip pickupSound;        // Sound that plays when wand is picked up

    private bool pickedUp = false;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            Debug.LogError(" WandPickup requires an XRGrabInteractable component!");

        // Auto-assign AudioSource if missing
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectEntered.AddListener(OnGrabAttempt);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectEntered.RemoveListener(OnGrabAttempt);
    }

    // Called when the wand is actually grabbed
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        TryPickup();
    }

    // Called before the wand is grabbed
    private void OnGrabAttempt(SelectEnterEventArgs args)
    {
        if (!EventManager.Instance.CanTriggerExternally(EventManager.GameEvent.WandPickedUp))
        {
            Debug.Log(" You can’t pick up the wand yet — open the chest first!");
            grabInteractable.interactionLayers = 0; // temporarily disable grabbing
            Invoke(nameof(ReenableGrab), 0.1f); // re-enable after short delay
        }
    }

    private void ReenableGrab()
    {
        grabInteractable.interactionLayers = -1; // re-enable all layers
    }

    // Non-VR testing via keyboard use keycode P
    void Update()
    {
        if (!pickedUp && Input.GetKeyDown(KeyCode.P))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (pickedUp) return;

        if (!EventManager.Instance.CanTriggerExternally(EventManager.GameEvent.WandPickedUp))
        {
            Debug.Log(" You can’t pick up the wand yet — open the chest first!");
            return;
        }

        pickedUp = true;
        Debug.Log(" Wand picked up!");
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.WandPickedUp);

        //  Play pickup sound
        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }
    }
}
// This code has been inspired by Copilot and ChatGPT.

