using UnityEngine;
using TMPro;

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

        // Removed WordCompleted event from here!
        // Now Cauldron will trigger it when all correct ingredients are added
    }

    // Reset letters if needed
    public void ResetLetters()
    {
        currentLetterIndex = 0;
        foreach (var field in letterFields)
            field.text = "_"; // or empty string
    }
}
