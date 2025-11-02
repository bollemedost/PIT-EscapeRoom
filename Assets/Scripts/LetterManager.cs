using UnityEngine;
using TMPro;

public class LetterManager : MonoBehaviour
{
    [Header("UI Text Fields for Letters")]
    public TMP_Text[] letterFields; // Assign your 4 text fields in Inspector
    public string secretWord = "KING"; // Your secret word

    [Header("Turbulence Settings")]
    public float amplitude = 5f;   // How strong the wobble is
    public float frequency = 5f;   // How fast it moves
    public float speed = 2f;       // Scroll speed of wave

    private int currentLetterIndex = 0;

    // Track letters that should be continuously animated
    private readonly System.Collections.Generic.List<TMP_Text> activeLetters = new();

    void Update()
    {
        AnimateLetters();
    }

    public void RevealNextLetter()
    {
        if (currentLetterIndex >= secretWord.Length) return;

        TMP_Text letterField = letterFields[currentLetterIndex];
        letterField.text = secretWord[currentLetterIndex].ToString();
        currentLetterIndex++;

        // Add this letter to the active list so it animates continuously
        if (!activeLetters.Contains(letterField))
            activeLetters.Add(letterField);
    }

    private void AnimateLetters()
    {
        foreach (var letter in activeLetters)
        {
            letter.ForceMeshUpdate();
            var mesh = letter.mesh;
            var vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector3(
                    Mathf.Sin(Time.time * frequency + i) * amplitude * 0.01f,
                    Mathf.Cos(Time.time * speed + i) * amplitude * 0.01f,
                    0
                );
            }

            mesh.vertices = vertices;
            letter.canvasRenderer.SetMesh(mesh);
        }
    }

    public void ResetLetters()
    {
        currentLetterIndex = 0;
        activeLetters.Clear();

        foreach (var field in letterFields)
            field.text = "_"; // or empty string
    }
}
