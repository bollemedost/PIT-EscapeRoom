using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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
        
        // Try to automatically connect the Close button (if it exists)
        Button closeButton = spawnedCanvas.GetComponentInChildren<Button>(true);
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonPressed);
        }

        spawnedCanvas.SetActive(true);
        isOpen = true;
    }

    private void HideCanvas()
    {
        if (spawnedCanvas != null)
        {
            Destroy(spawnedCanvas);
            spawnedCanvas = null;
        }

        isOpen = false;
    }

    // Called by the Close button on the canvas
    public void OnCloseButtonPressed()
    {
        HideCanvas();
    }
}
