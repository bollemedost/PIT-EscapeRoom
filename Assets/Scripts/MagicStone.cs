using UnityEngine;

public class MagicStone : MonoBehaviour
{
    [Header("Optional Effects")]
    public ParticleSystem appearParticles;    // For when stone appears
    public AudioSource appearSound;           // For when stone appears
    public ParticleSystem placedParticles;    // For when stone is placed
    public AudioSource placedSound;           // For when stone is placed

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

        // Force AudioSource to play even if it was disabled before
        if (appearSound != null)
        {
            appearSound.Stop();
            appearSound.Play();
        }

        // Force particle system to play
        if (appearParticles != null)
        {
            appearParticles.Clear();
            appearParticles.Play();
        }

        Debug.Log("✨ Magic Stone has appeared!");
    }


    private void OnStonePlaced()
    {
        if (placedParticles != null)
            placedParticles.Play();

        if (placedSound != null)
            placedSound.Play();

        Debug.Log("🎉 Stone placed! Victory effects triggered!");
    }
}
