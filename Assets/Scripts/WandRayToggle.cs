using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WandRayToggle : MonoBehaviour
{
    public GameObject wandRayRoot; // the "WandRay" object with XR Ray Interactor
    XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => { if (wandRayRoot) wandRayRoot.SetActive(true); });
        grab.selectExited.AddListener(_ => { if (wandRayRoot) wandRayRoot.SetActive(false); });
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveAllListeners();
        grab.selectExited.RemoveAllListeners();
    }
}

