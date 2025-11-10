using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeInSeconds = 300f; // 5 minutes
    public bool startOnAwake = true;

    [Header("UI References")]
    public TMP_Text timerText;         
    public GameObject timerCanvas;     
    public GameObject gameOverCanvas;  

    [Header("Scripts to Disable")]
    public List<MonoBehaviour> scriptsToDisable; // Assign scripts like EventManager

    [Header("Audio Settings")]
    public AudioSource audioSource;    
    public AudioClip alarmClip;        

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

            if (remainingTime < 0)
                remainingTime = 0;

            UpdateTimerUI();

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

        // Re-enable all scripts
        if (scriptsToDisable != null)
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (script != null)
                    script.enabled = true;
            }
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void TimerFinished()
    {
        if (!isTimerRunning) return;
        isTimerRunning = false;

        Debug.Log("⏰ Timer finished! You lose!");

        if (audioSource != null)
            audioSource.Stop();

        if (audioSource != null && alarmClip != null)
            audioSource.PlayOneShot(alarmClip);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Disable all assigned scripts
        if (scriptsToDisable != null)
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                if (script != null)
                    script.enabled = false;
            }
        }
    }
}
