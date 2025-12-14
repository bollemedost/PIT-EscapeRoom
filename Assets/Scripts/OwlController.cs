using System.Collections;
using UnityEngine;

public class OwlController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform[] flightPoints;
    public Transform noteDropPoint;
    public GameObject[] carriedNotes; // Assign all the notes in inspector

    [Header("Settings")]
    public float speed = 5f;

    [Header("Audio")]
    public AudioSource takeoffSound;
    public AudioSource flyingSound;
    public AudioSource dropSound;

    private bool isFlying = false;
    private int currentNoteIndex = 0;
    private bool noteDropped = false;


    // Call this to make the owl fly along its path.
    // Assigns the next note automatically.
    public void Fly()
    {
        if (!isFlying && flightPoints.Length > 0)
        {
            isFlying = true;
            noteDropped = false;

            if (takeoffSound != null) takeoffSound.Play();
            if (animator != null) animator.SetBool("isFlying", true);
            if (flyingSound != null) flyingSound.Play();

            StartCoroutine(FlyRoutine());
        }
    }

    private IEnumerator FlyRoutine()
    {
        foreach (Transform point in flightPoints)
        {
            while (Vector3.Distance(transform.position, point.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
                transform.LookAt(point);

                // Drop note at drop point
                if (!noteDropped && currentNoteIndex < carriedNotes.Length &&
                    carriedNotes[currentNoteIndex] != null &&
                    noteDropPoint != null &&
                    Vector3.Distance(transform.position, noteDropPoint.position) < 0.5f)
                {
                    DropNote(carriedNotes[currentNoteIndex]);
                    noteDropped = true;
                    currentNoteIndex++;
                }

                yield return null;
            }
            yield return null;
        }

        // Stop flying
        if (animator != null) animator.SetBool("isFlying", false);
        if (flyingSound != null) flyingSound.Stop();
        isFlying = false;
    }

    // Drops the given note naturally.
    private void DropNote(GameObject note)
    {
        if (note == null) return;

        note.transform.parent = null; // detach from owl

        Rigidbody rb = note.GetComponent<Rigidbody>();
        if (rb == null) rb = note.AddComponent<Rigidbody>();
        rb.isKinematic = false; // allow physics

        if (dropSound != null) dropSound.Play();
    }

    /// Resets all notes back to owl's hand.
    public void ResetNotes()
    {
        currentNoteIndex = 0;

        foreach (var note in carriedNotes)
        {
            if (note != null)
            {
                note.transform.parent = transform; // attach back to owl
                note.transform.localPosition = Vector3.zero; // reset position to owl hand

                Rigidbody rb = note.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = true; // stop physics while attached
            }
        }

        noteDropped = false;
    }
}
// This code has been inspired by Copilot and ChatGPT.

