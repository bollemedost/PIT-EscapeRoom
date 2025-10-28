using UnityEngine;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour
{
    [Header("Correct Ingredients")]
    public List<GameObject> correctIngredients; // rattale, spider, snakeskin, feather

    [Header("Effect Settings")]
    public float rejectForce = 8f; // upward force for wrong ingredients
    public float rejectOutwardForce = 4f; // sideways force

    [Header("Audio & Visual Effects")]
    public AudioSource audioSource;        // assign an AudioSource on the cauldron
    public AudioClip correctSound;         // bubbling, sparkle, etc.
    public AudioClip wrongSound;           // splat, hiss, or pop sound
    public ParticleSystem correctParticles; // optional: sparkle/smoke when correct

    [Header("Letter Reveal UI")]
    public LetterManager letterManager; // optional: canvas to reveal letters

    private void OnTriggerEnter(Collider other)
    {
        GameObject ingredient = other.gameObject;

        if (IsCorrectIngredient(ingredient))
        {
            KeepIngredient(ingredient);
            PlayCorrectEffects();

            if (letterManager != null)
                letterManager.RevealNextLetter();

            EventManager.Instance.TriggerEvent(EventManager.GameEvent.CorrectIngredientAdded);
        }
        else
        {
            RejectIngredient(ingredient);
            PlayWrongEffects();
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.WrongIngredientAdded);
        }
    }

    // Check if ingredient is in the correct list
    private bool IsCorrectIngredient(GameObject ingredient)
    {
        return correctIngredients.Contains(ingredient);
    }

    // Keep correct ingredient inside the cauldron
    private void KeepIngredient(GameObject ingredient)
    {
        ingredient.transform.SetParent(transform);
        ingredient.transform.localPosition = Vector3.zero;

        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb)
            rb.isKinematic = true;
    }

    // Spit out wrong ingredient instantly
   private void RejectIngredient(GameObject ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb)
        {
            // Make sure physics is active
            rb.isKinematic = false;

            // Optional: lift it slightly so it doesn't intersect the cauldron
            ingredient.transform.position += Vector3.up * 0.2f;

            // Apply upward + random outward force immediately
            Vector3 forceDir = (Vector3.up + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f))).normalized;
            rb.AddForce(forceDir * rejectForce, ForceMode.Impulse);
        }
    }


    // --- Visual & Sound Effects ---
    private void PlayCorrectEffects()
    {
        if (audioSource != null && correctSound != null)
            audioSource.PlayOneShot(correctSound);

        if (correctParticles != null)
            correctParticles.Play();
    }

    private void PlayWrongEffects()
    {
        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);
    }
}
