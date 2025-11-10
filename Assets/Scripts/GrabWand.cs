using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class OneTimeGrabXRI : MonoBehaviour
{
    XRGrabInteractable grab;
    Rigidbody rb;
    bool locked;

    [Header("Switch to wand on equip")]
    [Tooltip("Right hand near interactor used for close grabs (Direct Interactor). Disable this after wand is equipped.")]
    public XRBaseInteractor rightHandDirect;

    [Tooltip("Right hand ray/near-far interactor (optional). We'll strip its pickup layer after equip so it won't grab items from afar.")]
    public XRBaseInteractor rightHandRay;

    [Tooltip("The XR Direct Interactor on the wand tip. Keep this disabled at start; we enable it on equip.")]
    public XRDirectInteractor wandTip;

    [Tooltip("Layer(s) for shared grabbables (e.g., HandAndWandPickup). Make sure wandTip uses this.")]
    public InteractionLayerMask sharedPickupMask;

    [Tooltip("Layers for the hand AFTER the wand is equipped (usually None or only UI/Teleport).")]
    public InteractionLayerMask handAfterMask;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        grab.selectEntered.AddListener(OnGrab);

        // Start: tip off (so you can't grab with wand before it's picked up)
        if (wandTip) wandTip.enabled = false;
    }

    void OnDestroy()
    {
        if (grab != null) grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (locked) return;
        locked = true;

        var interactor = args.interactorObject as IXRSelectInteractor;
        var handAttach = interactor.GetAttachTransform(grab);

        // Lock physics so it sticks
        rb.isKinematic = true;
        rb.useGravity = false;

        // Switch input authority to wand (disable hand, enable tip)
        SwitchControlToWand();

        StartCoroutine(FreeHandThenStick(interactor, handAttach));
    }

    void SwitchControlToWand()
    {
        // End any current hand-held object
        if (rightHandDirect && rightHandDirect.hasSelection)
            rightHandDirect.EndManualInteraction();
        if (rightHandRay && rightHandRay.hasSelection)
            rightHandRay.EndManualInteraction();

        // Disable hand direct grabbing (simplest + robust)
        if (rightHandDirect) rightHandDirect.enabled = false;

        // Make sure the ray cannot grab shared pickups anymore (keeps it for UI if you want)
        if (rightHandRay) rightHandRay.interactionLayers = handAfterMask;

        // Enable wand tip and give it the shared pickup mask
        if (wandTip)
        {
            wandTip.interactionLayers = sharedPickupMask;
            wandTip.enabled = true;
        }
    }

    IEnumerator FreeHandThenStick(IXRSelectInteractor interactor, Transform handAttach)
    {
        // Let XRI finish the initial select bookkeeping
        yield return null;

        // Release the grab so we can permanently parent it
        if (grab != null && grab.interactionManager != null)
            grab.interactionManager.SelectExit(interactor, grab);

        yield return null;

        rb.isKinematic = true;
        rb.useGravity = false;

        // Use the wand's Attach Transform if set (GripPointR), else root
        Transform itemAttach = grab != null && grab.attachTransform != null ? grab.attachTransform : transform;

        // Compute the wand root's world pose such that itemAttach == handAttach (same pos/rot)
        Quaternion rootRot = handAttach.rotation * Quaternion.Inverse(itemAttach.localRotation);
        Vector3 rootPos = handAttach.position - (rootRot * itemAttach.localPosition);

        // Apply world pose first (before parenting), so there is no drift
        transform.SetPositionAndRotation(rootPos, rootRot);

        // Now parent to the hand while preserving the world pose we just set
        transform.SetParent(handAttach, true);

        // Prevent future grabs
        if (grab != null) Destroy(grab);
    }
}
