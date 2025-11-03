using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WandGrabByAction : MonoBehaviour
{
    [Header("Hook up the same action used by RightHand → Animation Controller → Grab Action")]
    public InputActionReference grabAction;

    [Header("Interactor on the wand tip (Ray or Direct)")]
    public XRBaseInteractor wandInteractor;

    [Tooltip("If true, press-to-toggle (sticky). If false, hold-to-grab.")]
    public bool stickySelect = true;

    private bool isHeldDown;
    private readonly List<IXRInteractable> _validTargets = new();

    void OnEnable()
    {
        if (grabAction != null)
        {
            grabAction.action.performed += OnGrabPerformed;
            grabAction.action.canceled += OnGrabCanceled;
        }
    }

    void OnDisable()
    {
        if (grabAction != null)
        {
            grabAction.action.performed -= OnGrabPerformed;
            grabAction.action.canceled -= OnGrabCanceled;
        }
    }

    void OnGrabPerformed(InputAction.CallbackContext ctx)
    {
        if (wandInteractor == null) return;

        if (stickySelect)
        {
            if (wandInteractor.hasSelection)
            {
                // Toggle off: end current selection
                wandInteractor.EndManualInteraction(); // <-- no args in XRIT 3.x
            }
            else
            {
                // Toggle on: pick the best valid target
                IXRSelectInteractable target = GetBestValidTarget();
                if (target != null)
                    wandInteractor.StartManualInteraction(target);
            }
        }
        else
        {
            // Hold-to-grab behavior
            isHeldDown = true;
            if (!wandInteractor.hasSelection)
            {
                IXRSelectInteractable target = GetBestValidTarget();
                if (target != null)
                    wandInteractor.StartManualInteraction(target);
            }
        }
    }

    void OnGrabCanceled(InputAction.CallbackContext ctx)
    {
        if (!stickySelect && isHeldDown && wandInteractor != null && wandInteractor.hasSelection)
        {
            // Release on button up
            wandInteractor.EndManualInteraction(); // <-- no args
        }
        isHeldDown = false;
    }

    IXRSelectInteractable GetBestValidTarget()
    {
        _validTargets.Clear();
        wandInteractor.GetValidTargets(_validTargets);
        for (int i = 0; i < _validTargets.Count; i++)
        {
            if (_validTargets[i] is IXRSelectInteractable sel)
                return sel; // first valid
        }
        return null;
    }
}
