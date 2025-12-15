using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false; // Prevents reopening chest

    [Header("Code Canvas Setup")]
    public CodePanel codePanel;

    [Header("Audio Settings")]
    public AudioSource audioSource;   
    public AudioClip openChestSound;  // Sound when chest opens
    public AudioClip correctCodeSound; 

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("No Animator found on the Chest!");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // For testing in scene view use the E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowCodePanel();
        }
    }

    // Show the CodePanel
    public void ShowCodePanel()
    {
        if (isOpened)
            return;

        if (codePanel != null)
            codePanel.TogglePanel();
    }

    // Called when the player enters the correct code
    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        // Plays the correct code sound first
        if (audioSource != null && correctCodeSound != null)
            audioSource.PlayOneShot(correctCodeSound);

        // Then it plays the chest open sound after
        if (audioSource != null && openChestSound != null)
            audioSource.PlayOneShot(openChestSound);

        // Disable the CodePanel permanently to prevent further interaction
        if (codePanel != null)
            codePanel.ClosePanel();

        // Trigger chest opened event in the EventManager
        if (EventManager.Instance != null)
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChestOpened);
    }
}
// This code has been inspired by Copilot and ChatGPT.

