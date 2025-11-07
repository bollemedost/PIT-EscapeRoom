using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;

    [Header("Code Canvas Setup")]
    public CodePanel codePanel; // assign the CodePanel from scene

    [Header("Audio Settings")]
    public AudioSource audioSource;   // Source to play the sound
    public AudioClip openChestSound;  // Sound that plays when the chest opens

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
        if (codePanel != null)
        {
            codePanel.TogglePanel();
        }
    }

    /// <summary>
    /// Called when the player enters the correct code
    /// </summary>
    public void OpenChest()
    {
        if (animator != null)
            animator.SetTrigger("Open");

        if (audioSource != null && openChestSound != null)
            audioSource.PlayOneShot(openChestSound);

        // ✅ Trigger the "ChestOpened" event so SubstituteObjectController knows to swap the wand
        if (EventManager.Instance != null)
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChestOpened);
    }
}
