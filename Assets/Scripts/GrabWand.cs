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
        var attach = interactor.GetAttachTransform(grab);

        // In case inspector wasn’t set exactly
        rb.isKinematic = true;
        rb.useGravity = false;

        StartCoroutine(FreeHandThenStick(interactor, attach));
    }

    IEnumerator FreeHandThenStick(IXRSelectInteractor interactor, Transform attach)
    {
        yield return null; // let XRI finish initial select this frame

        // 1) Free the grab button
        grab.interactionManager.SelectExit(interactor, grab);

        yield return null; // wait a frame so XRI finishes its release bookkeeping

        // 2) Make sure physics stays locked (XRI can flip these on release otherwise)
        rb.isKinematic = true;
        rb.useGravity = false;

        // 3) Parent to the hand’s attach point and align
        transform.SetParent(attach, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 4) Prevent future grabs
        Destroy(grab);
    }
}
