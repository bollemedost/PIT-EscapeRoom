using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Recipe : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject recipeCanvasPrefab; // Assign recipe canvas prefab here
    private GameObject spawnedCanvas;

    [Header("VR Interaction")]
    public XRBaseInteractor interactor; //not used but could be for future extensions for haptic feedback

    [Header("Audio")]
    public AudioSource audioSource;  
    public AudioClip interactionClip;

    private bool isOpen = false;

    void Update()
    {
        // For testing in Unity without VR use the R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleCanvas();
            PlayInteractionSound();
        }
    }

    // Called by VR interaction 
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        ToggleCanvas();
        PlayInteractionSound();

        // Not used but optional for future improvements like haptic feedback for VR controller
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

    // Plays sound when the object is interacted with
    private void PlayInteractionSound()
    {
        if (audioSource != null)
        {
            if (interactionClip != null)
            {
                audioSource.clip = interactionClip;
                audioSource.Play();
            }
            else
            {
                audioSource.Play(); // Plays whatever clip is already on the AudioSource
            }
        }
        else
        {
            Debug.LogWarning("No AudioSource assigned to Recipe script!");
        }
    }
}
// This code has been inspired by Copilot and ChatGPT.

