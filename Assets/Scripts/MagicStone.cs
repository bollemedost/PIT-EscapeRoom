using UnityEngine;

public class MagicStone : MonoBehaviour
{
    [Header("Optional Effects")]
    public ParticleSystem appearParticles;    // Particle for when stone appears
    public AudioSource appearSound;           // Audio For when stone appears
    public ParticleSystem placedParticles;    // Particle for when stone is placed
    public AudioSource placedSound;           // Audio For when stone is placed

    private void Awake()
    {
        gameObject.SetActive(false);

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
        switch (evt)
        {
            case EventManager.GameEvent.MagicStoneAppeared:
                ShowStone();
                break;
            case EventManager.GameEvent.StonePlaced:
                OnStonePlaced();
                break;
        }
    }

    private void ShowStone()
    {
        gameObject.SetActive(true);

        if (appearSound != null)
        {
            appearSound.Stop();
            appearSound.Play();
        }

        if (appearParticles != null)
        {
            appearParticles.Clear();
            appearParticles.Play();
        }

        Debug.Log("Magic Stone has appeared");
    }


    private void OnStonePlaced()
    {
        if (placedParticles != null)
            placedParticles.Play();

        if (placedSound != null)
            placedSound.Play();

        Debug.Log("Stone placed!");
    }
}
// This code has been inspired by Copilot and ChatGPT.

