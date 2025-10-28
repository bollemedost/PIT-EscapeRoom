using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit; // for haptics

public class CodePanel : MonoBehaviour
{
    public TMP_Text[] digitFields; // assign your 4 text fields
    public string correctCode = "1234";
    private string currentCode = "";

    public Chest chestToOpen; // assign the chest

    [Header("Feedback")]
    public AudioSource audioSource;       // assign an AudioSource on the canvas or parent
    public AudioClip correctClip;
    public AudioClip wrongClip;

    public XRBaseController rightHandController; // assign XR controller for haptics (optional)
    public float vibrationDuration = 0.2f;
    public float vibrationAmplitude = 0.5f;

    public float delayBeforeCheck = 0.5f; // delay so fourth digit is visible

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

            // Wait a bit so the sound can finish before hiding
            yield return new WaitForSeconds(1.0f);

            gameObject.SetActive(false); // now hide the code panel
        }
        else
        {
            Debug.Log("❌ Wrong code!");

            if (audioSource != null && wrongClip != null)
                audioSource.PlayOneShot(wrongClip);

            if (rightHandController != null)
                rightHandController.SendHapticImpulse(vibrationAmplitude, vibrationDuration);

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
        Debug.Log("❌ Code panel closed by player");
        gameObject.SetActive(false);
    }

}
