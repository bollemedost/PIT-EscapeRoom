using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors; // for VR interactions


public class Recipe : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject recipeCanvasPrefab; // Assign your recipe canvas prefab here
    private GameObject spawnedCanvas;

    [Header("VR Interaction")]
    public XRBaseInteractor interactor; // Optional: assign if you want haptic feedback

    private bool isOpen = false;

    void Update()
    {
        // For testing in Unity without VR
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleCanvas();
        }
    }

    // Called by VR interaction (e.g., XR Grab or XR Simple Interactable)
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleCanvas();

        // Optional: haptic feedback for VR controller
        if (args.interactorObject is XRBaseInputInteractor controllerInteractor)
        {
            controllerInteractor.SendHapticImpulse(0.5f, 0.2f);
        }
    }

    private void ToggleCanvas()
    {
        if (!isOpen)
        {
            ShowCanvas();
        }
        else
        {
            HideCanvas();
        }
    }

    private void ShowCanvas()
    {
        if (spawnedCanvas != null) return; // already open

        // Instantiate the canvas
        spawnedCanvas = Instantiate(recipeCanvasPrefab);

        // Parent to camera for VR or leave in world space for 2D view
        Transform cameraTransform = Camera.main.transform;
        spawnedCanvas.transform.SetParent(cameraTransform);

        // Position slightly in front of player
        spawnedCanvas.transform.localPosition = new Vector3(0, 0, 3f);
        spawnedCanvas.transform.localRotation = Quaternion.identity;

        spawnedCanvas.SetActive(true);
        isOpen = true;
    }

    private void HideCanvas()
    {
        if (spawnedCanvas != null)
            Destroy(spawnedCanvas);

        isOpen = false;
    }
}
