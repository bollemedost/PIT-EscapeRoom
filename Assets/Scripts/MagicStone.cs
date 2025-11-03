using UnityEngine;

public class MagicStone : MonoBehaviour
{
    [Header("Optional Effects")]
    public ParticleSystem appearParticles;
    public AudioSource appearSound; // <- assign the AudioSource component here in the Inspector

    private void Awake()
    {
        // Make sure the stone starts inactive
        gameObject.SetActive(false);

        // Subscribe to EventManager
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered += OnEventTriggered;
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered -= OnEventTriggered;
    }

    private void OnEventTriggered(EventManager.GameEvent evt)
    {
        if (evt == EventManager.GameEvent.MagicStoneAppeared)
        {
            ShowStone();
        }
    }

    private void ShowStone()
    {
        gameObject.SetActive(true);

        // Play optional effects
        if (appearParticles != null)
            appearParticles.Play();

        if (appearSound != null)
            appearSound.Play(); // plays the AudioSource’s assigned clip

        Debug.Log("✨ Magic Stone has appeared!");
    }
}
