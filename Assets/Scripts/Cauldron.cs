using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Header("Correct Ingredients")]
    public List<GameObject> correctIngredients;

    [Header("Effect Settings")]
    public float rejectForce = 8f;
    public float rejectOutwardForce = 4f;

    [Header("Audio & Visual Effects")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    [Header("Particle Prefabs")]
    public GameObject correctParticlePrefab;
    public GameObject wrongParticlePrefab;

    [Header("Letter Reveal UI")]
    public LetterManager letterManager;

    [Header("Wand Settings")]
    public string wandTag = "Wand"; // Set this to wand’s tag in the Inspector

    private HashSet<GameObject> addedIngredients = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject ingredient = other.gameObject;

        // Ignore wand completely
        if (ingredient.CompareTag(wandTag))
            return;

        if (addedIngredients.Contains(ingredient)) return;
        addedIngredients.Add(ingredient);

        if (IsCorrectIngredient(ingredient))
        {
            KeepIngredient(ingredient);
            PlayCorrectEffects(ingredient);

            if (letterManager != null)
                letterManager.RevealNextLetter();

            EventManager.Instance.TriggerEvent(EventManager.GameEvent.CorrectIngredientAdded);

            if (AllCorrectIngredientsAdded())
                EventManager.Instance.TriggerEvent(EventManager.GameEvent.WordCompleted);
        }
        else
        {
            RejectIngredient(ingredient);
            PlayWrongEffects(ingredient);
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

    private void PlayCorrectEffects(GameObject ingredient)
    {
        if (audioSource != null && correctSound != null)
            audioSource.PlayOneShot(correctSound);

        if (correctParticlePrefab != null)
        {
            GameObject particles = Instantiate(
                correctParticlePrefab,
                ingredient.transform.position,
                Quaternion.identity
            );

            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(FadeOutParticles(ps, 2f));
                Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax + 1f);
            }
            else
            {
                Destroy(particles, 10f);
            }
        }
    }

    private void PlayWrongEffects(GameObject ingredient)
    {
        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);

        if (wrongParticlePrefab != null)
        {
            GameObject particles = Instantiate(
                wrongParticlePrefab,
                ingredient.transform.position,
                Quaternion.identity
            );

            ParticleSystem ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(FadeOutParticles(ps, 2f));
                Destroy(particles, ps.main.duration + ps.main.startLifetime.constantMax + 1f);
            }
            else
            {
                Destroy(particles, 10f);
            }
        }
    }

    private IEnumerator FadeOutParticles(ParticleSystem ps, float fadeDuration)
    {
        var main = ps.main;
        Color startColor = main.startColor.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            main.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        ps.Stop();
    }
}
// This code has been inspired by Copilot and ChatGPT.
