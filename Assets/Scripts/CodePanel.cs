using UnityEngine;
using TMPro;
using System.Collections;

public class CodePanel : MonoBehaviour
{
    [Header("Core")]
    public TMP_Text[] digitFields; // assign your 4 text fields
    public string correctCode = "1234";
    private string currentCode = "";

    public Chest chestToOpen; // assign the chest

    [Header("Feedback")]
    public AudioSource audioSource;       // assign an AudioSource on the canvas or parent
    public AudioClip correctClip;
    public AudioClip wrongClip;

    public float delayBeforeCheck = 0.5f; // delay so fourth digit is visible

    private bool isPanelVisible = false; // track visibility

    void Start()
    {
        // Hide panel at start
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Toggle panel on/off
    /// </summary>
    public void TogglePanel()
    {
        isPanelVisible = !isPanelVisible;
        gameObject.SetActive(isPanelVisible);

        if (!isPanelVisible)
        {
            ResetCode();
        }
    }

    // Called by each number button
    public void AddDigit(string digit)
    {
        if (currentCode.Length >= 4) return;

        currentCode += digit;
        digitFields[currentCode.Length - 1].text = digit;

        if (currentCode.Length == 4)
            StartCoroutine(DelayCheck());
    }

    private IEnumerator DelayCheck()
    {
        yield return new WaitForSeconds(delayBeforeCheck);

        if (currentCode == correctCode)
        {
            Debug.Log("✅ Correct code!");

            if (audioSource != null && correctClip != null)
                audioSource.PlayOneShot(correctClip);

            if (chestToOpen != null)
                chestToOpen.OpenChest();

            yield return new WaitForSeconds(1.0f);

            TogglePanel(); // hide panel after correct code
        }
        else
        {
            Debug.Log("❌ Wrong code!");

            if (audioSource != null && wrongClip != null)
                audioSource.PlayOneShot(wrongClip);

            ResetCode();
        }
    }

    public void ResetCode()
    {
        currentCode = "";
        foreach (var field in digitFields)
            field.text = "";
    }

    public void ClosePanel()
    {
        isPanelVisible = false;
        gameObject.SetActive(false);
        ResetCode();
    }
}
