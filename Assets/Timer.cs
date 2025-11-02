using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeInSeconds = 300f; // e.g., 5 minutes
    public bool startOnAwake = true;

    [Header("UI References")]
    public TMP_Text timerText;            // Assign your TextMeshProUGUI component
    public GameObject timerCanvas;        // The canvas that shows the timer
    public GameObject gameOverCanvas;     // The canvas that says "You Lose"

    [Header("XR Player References")]
    public GameObject leftHand;           // Assign your LeftHand Controller
    public GameObject rightHand;          // Assign your RightHand Controller

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
    }

    void Update()
    {
        if (!isTimerRunning)
            return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                TimerFinished();
            }
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
    }

    public void ResetTimer()
    {
        remainingTime = timeInSeconds;
        isTimerRunning = true;
        UpdateTimerUI();

        // Reactivate timer canvas & hide game over canvas
        if (timerCanvas != null)
            timerCanvas.SetActive(true);
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        // Re-enable hands
        if (leftHand) leftHand.SetActive(true);
        if (rightHand) rightHand.SetActive(true);
    }

    private void TimerFinished()
    {
        isTimerRunning = false;
        Debug.Log("⏰ Timer finished! You lose!");

        // Show Game Over UI
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Hide timer UI
        if (timerCanvas != null)
            timerCanvas.SetActive(false);

        // Disable hands
        if (leftHand) leftHand.SetActive(false);
        if (rightHand) rightHand.SetActive(false);
    }
}
