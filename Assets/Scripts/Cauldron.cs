using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Header("Correct Ingredients")]
    public List<GameObject> correctIngredients; // rattale, spider, snakeskin, feather

    [Header("Effect Settings")]
    public float rejectForce = 8f;           
    public float rejectOutwardForce = 4f;    

    [Header("Audio & Visual Effects")]
    public AudioSource audioSource;          
    public AudioClip correctSound;           
    public AudioClip wrongSound;             
    public ParticleSystem correctParticles;  

    [Header("Letter Reveal UI")]
    public LetterManager letterManager;      

    // Track ingredients already added
    private HashSet<GameObject> addedIngredients = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject ingredient = other.gameObject;

        // Only trigger once per ingredient
        if (addedIngredients.Contains(ingredient)) return;
        addedIngredients.Add(ingredient);

        if (IsCorrectIngredient(ingredient))
        {
            KeepIngredient(ingredient);
            PlayCorrectEffects();

            if (letterManager != null)
                letterManager.RevealNextLetter();

            EventManager.Instance.TriggerEvent(EventManager.GameEvent.CorrectIngredientAdded);

            // Check if all correct ingredients are added
            if (AllCorrectIngredientsAdded())
            {
                EventManager.Instance.TriggerEvent(EventManager.GameEvent.WordCompleted);
            }
        }
        else
        {
            RejectIngredient(ingredient);
            PlayWrongEffects();

            EventManager.Instance.TriggerEvent(EventManager.GameEvent.WrongIngredientAdded);
        }
    }

    private bool IsCorrectIngredient(GameObject ingredient)
    {
        return correctIngredients.Contains(ingredient);
    }

    private bool AllCorrectIngredientsAdded()
    {
        foreach (var ing in correctIngredients)
        {
            if (!addedIngredients.Contains(ing))
                return false;
        }
        return true;
    }

    private void KeepIngredient(GameObject ingredient)
    {
        ingredient.transform.SetParent(transform);
        ingredient.transform.localPosition = Vector3.zero;

        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        // Disable collider so it cannot trigger again
        Collider col = ingredient.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    private void RejectIngredient(GameObject ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            ingredient.transform.position += Vector3.up * 0.2f;

            Vector3 forceDir = (Vector3.up + new Vector3(
                Random.Range(-0.5f, 0.5f),
                0,
                Random.Range(-0.5f, 0.5f))).normalized;

            rb.AddForce(forceDir * rejectForce, ForceMode.Impulse);
        }
    }

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
