using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false; // ✅ Prevents reopening

    [Header("Code Canvas Setup")]
    public CodePanel codePanel; // assign the CodePanel from scene

    [Header("Audio Settings")]
    public AudioSource audioSource;   // Source to play sounds
    public AudioClip openChestSound;  // Sound when chest opens
    public AudioClip correctCodeSound; // ✅ New: plays when correct code entered

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("❌ No Animator found on the Chest!");

        // Auto-assign AudioSource if missing
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Optional: For desktop testing only
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowCodePanel();
        }
    }

    /// <summary>
    /// Show the CodePanel (does NOT move it)
    /// </summary>
    public void ShowCodePanel()
    {
        if (isOpened)
            return;

        if (codePanel != null)
            codePanel.TogglePanel();
    }

    /// <summary>
    /// Called when the player enters the correct code
    /// </summary>
    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        if (animator != null)
            animator.SetTrigger("Open");

        // ✅ Play correct code sound first (if assigned)
        if (audioSource != null && correctCodeSound != null)
            audioSource.PlayOneShot(correctCodeSound);

        // ✅ Then play the chest open sound slightly after
        if (audioSource != null && openChestSound != null)
            audioSource.PlayOneShot(openChestSound);

        // ✅ Disable the CodePanel permanently
        if (codePanel != null)
            codePanel.ClosePanel();

        // ✅ Trigger chest opened event
        if (EventManager.Instance != null)
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChestOpened);
    }
}
