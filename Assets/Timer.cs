using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeInSeconds = 300f; // 5 minutes
    public bool startOnAwake = true;

    [Header("UI References")]
    public TMP_Text timerText;          // Assign your TextMeshProUGUI component
    public GameObject timerCanvas;      // The canvas that shows the timer
    public GameObject gameOverCanvas;   // The canvas that says "You Lose"

    [Header("XR Player References")]
    public GameObject leftHand;         // Assign your LeftHand Controller
    public GameObject rightHand;        // Assign your RightHand Controller

    [Header("Audio Settings")]
    public AudioSource audioSource;     // AudioSource for ticking and alarm
    public AudioClip alarmClip;         // Sound that plays when time runs out

    private float remainingTime;
    private bool isTimerRunning = false;

    void Start()
    {
        remainingTime = timeInSeconds;
        UpdateTimerUI();

        if (startOnAwake)
            StartTimer();

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        // Start ticking sound (looping)
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // Clamp to zero to avoid negatives
            if (remainingTime < 0)
                remainingTime = 0;

            UpdateTimerUI();

            // Stop when reaching exactly zero
            if (Mathf.Approximately(remainingTime, 0))
                TimerFinished();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        remainingTime = timeInSeconds;
        isTimerRunning = true;

        // Start ticking sound if not already playing
        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void ResetTimer()
    {
        remainingTime = timeInSeconds;
        isTimerRunning = true;
        UpdateTimerUI();

        if (timerCanvas != null)
            timerCanvas.SetActive(true);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (leftHand) leftHand.SetActive(true);
        if (rightHand) rightHand.SetActive(true);

        // Restart ticking sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void TimerFinished()
    {
        if (!isTimerRunning) return; // Prevent double calls
        isTimerRunning = false;

        Debug.Log("⏰ Timer finished! You lose!");

        // Stop ticking
        if (audioSource != null)
            audioSource.Stop();

        // Play alarm once
        if (audioSource != null && alarmClip != null)
            audioSource.PlayOneShot(alarmClip);

        // ✅ Keep timer visible
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Disable hands
        if (leftHand) leftHand.SetActive(false);
        if (rightHand) rightHand.SetActive(false);
    }
}
