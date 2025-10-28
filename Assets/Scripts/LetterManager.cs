using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LetterManager : MonoBehaviour
{
    [Header("UI Text Fields for Letters")]
    public TMP_Text[] letterFields; // assign 4 text fields in inspector
    public string secretWord = "KING"; // your secret word

    private int currentLetterIndex = 0;

    // Call this when a correct ingredient is added
    public void RevealNextLetter()
    {
        if (currentLetterIndex >= secretWord.Length) return;

        letterFields[currentLetterIndex].text = secretWord[currentLetterIndex].ToString();
        currentLetterIndex++;

        // Optional: trigger event when full word is completed
        if (currentLetterIndex == secretWord.Length)
        {
            Debug.Log("🎉 Word completed!");
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.WordCompleted);
        }
    }

    // Reset letters if needed
    public void ResetLetters()
    {
        currentLetterIndex = 0;
        foreach (var field in letterFields)
            field.text = "_"; // or empty string
    }
}
