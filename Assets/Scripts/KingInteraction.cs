using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class KingInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject whiteKing; // assign the white king gameobject in inspector
    public Transform moveTarget; // target where the black king should move to
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip fallSound;

    private bool hasInteracted = false;

    private void Start()
    {
        if (whiteKing == null)
            Debug.LogWarning("White king not assigned in KingInteraction script.");
    }

    private void Update()
    {
        // For testing in scene view use the K key
        if (Input.GetKeyDown(KeyCode.K))
            StartCoroutine(TriggerChessEvent());
    }

    // Called when the black king is touched or pressed in VR
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!hasInteracted)
        {
            hasInteracted = true;
            StartCoroutine(TriggerChessEvent());
        }
    }

    private IEnumerator TriggerChessEvent()
    {
        Debug.Log("Black King interacted with!");

        // Play move sound
        if (audioSource && moveSound)
            audioSource.PlayOneShot(moveSound);

        // Move black king slightly
        if (moveTarget != null)
        {
            float duration = 1f;
            Vector3 startPos = transform.position;
            Vector3 endPos = moveTarget.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
        }

        yield return new WaitForSeconds(0.5f);

        // Make white king fall
        if (whiteKing != null)
        {
            Rigidbody rb = whiteKing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(Vector3.forward * 2f, ForceMode.Impulse);

                if (audioSource && fallSound)
                    audioSource.PlayOneShot(fallSound);
            }
        }

        // update game state via event manager
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChessKingMoved);
    }
}
// This code has been inspired by Copilot and ChatGPT.

