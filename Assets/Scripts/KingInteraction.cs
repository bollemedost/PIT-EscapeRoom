using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class KingInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject whiteKing; // assign in inspector
    public Transform moveTarget; // where the black king should move to (optional)
    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip fallSound;

    private bool hasInteracted = false;

    private void Start()
    {
        // Optional safety check
        if (whiteKing == null)
            Debug.LogWarning("⚠️ White king not assigned in KingInteraction script.");
    }

        private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            StartCoroutine(TriggerChessEvent());
    }

    // Called when touched or pressed in VR
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
        Debug.Log("♟️ Black King interacted with!");

        // Play move sound
        if (audioSource && moveSound)
            audioSource.PlayOneShot(moveSound);

        // Move black king slightly (optional animation/move)
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

        // Optionally trigger your event manager
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChessKingMoved);
    }
}
