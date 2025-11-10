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

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        grab.selectEntered.AddListener(OnGrab);
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

        // Make sure physics doesn't interfere
        rb.isKinematic = true;
        rb.useGravity = false;

        StartCoroutine(FreeHandThenStick(interactor, handAttach));
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

        // ----- KEY PART: align using WORLD SPACE so the handle lands in the hand -----
        // Use the wand's Attach Transform if set (GripPointR). Otherwise fall back to root.
        Transform itemAttach = grab != null && grab.attachTransform != null ? grab.attachTransform : transform;

        // Compute the wand root's world pose such that itemAttach == handAttach (same pos/rot)
        Quaternion rootRot = handAttach.rotation * Quaternion.Inverse(itemAttach.localRotation);
        Vector3 rootPos = handAttach.position - (rootRot * itemAttach.localPosition);

        // Apply world pose first (before parenting), so there is no drift
        transform.SetPositionAndRotation(rootPos, rootRot);

        // Now parent to the hand while preserving the world pose we just set
        transform.SetParent(handAttach, true);

        // Optional: sanity check (should be ~zero)
        // Debug.Log("Post-align local offset of attach: " + handAttach.InverseTransformPoint(itemAttach.position));

        // One-time grab: prevent future grabs
        if (grab != null) Destroy(grab);
    }
}
