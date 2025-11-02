using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [Header("Hints Configuration")]
    public List<EventHints> eventHintsList; // Assign 2+ hints per event

    [Header("UI References")]
    public Button hintButton; // Optional: Button to show hint
    public Animator hintButtonAnimator; // Animator for the hint button
    public string hintAvailableBool = "HintAvailableBool"; // Animator bool name

    [Header("Hint Settings")]
    public float hintDuration = 3f; // seconds each hint stays visible
    public float hintCooldown = 5f; // minimum time between hints

    [Header("Initial Hint Before Any Event")]
    public GameObject initialHint; // The first hint visible before any event starts

    private EventManager.GameEvent currentEvent;
    private int hintsUsed = 0; // Tracks which hint to show next
    private Coroutine hintCoroutine;
    private EventHints currentHints; // Holds the hints for the active event
    private bool canShowNextHint = true; // cooldown flag
    private bool initialHintUsed = false; // Tracks if the initial hint has been used

    void Start()
    {
        EventManager.Instance.OnEventTriggered += OnEventTriggered;

        if (hintButton != null)
            hintButton.onClick.AddListener(OnHintButtonClicked);

        HideAllHints();

        // Initial hint setup
        if (initialHint != null)
        {
            initialHint.SetActive(false);
            initialHintUsed = false;
            canShowNextHint = true;

            // Play animation for initial hint availability
            if (hintButtonAnimator != null)
                hintButtonAnimator.SetBool(hintAvailableBool, true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ShowHint();
    }

    void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered -= OnEventTriggered;

        if (hintButton != null)
            hintButton.onClick.RemoveListener(OnHintButtonClicked);
    }

    private void OnEventTriggered(EventManager.GameEvent evt)
    {
        currentEvent = evt;
        hintsUsed = 0; // reset counter for new event

        // Find hints for this event
        currentHints = eventHintsList.Find(e => e.eventType == currentEvent);
        HideAllHints();
        canShowNextHint = false; // optional: prevent instant hint after event trigger

        // Start a coroutine to enable hints after cooldown
        StartCoroutine(EventHintAvailabilityCoroutine());
    }

    private IEnumerator EventHintAvailabilityCoroutine()
    {
        yield return new WaitForSeconds(hintCooldown); // delay before hint becomes available
        canShowNextHint = true;

        if (hintButtonAnimator != null && HasNextHint())
            hintButtonAnimator.SetBool(hintAvailableBool, true);
    }

    private void OnHintButtonClicked()
    {
        ShowHint();
    }

    public void ShowHint()
    {
        if (!canShowNextHint) return;

        // Stop button animation when hint is clicked
        if (hintButtonAnimator != null)
            hintButtonAnimator.SetBool(hintAvailableBool, false);

        // Show initial hint if not used yet
        if (!initialHintUsed && initialHint != null)
        {
            if (hintCoroutine != null)
                StopCoroutine(hintCoroutine);
            hintCoroutine = StartCoroutine(ShowHintCoroutine(initialHint));

            initialHintUsed = true;
            canShowNextHint = false;

            StartCoroutine(HintCooldownCoroutine());
            return;
        }

        if (currentHints == null || currentHints.hints.Count == 0) return;
        if (hintsUsed >= currentHints.hints.Count)
        {
            Debug.Log("No more hints available for this event!");
            return;
        }

        GameObject nextHint = currentHints.hints[hintsUsed];
        if (nextHint != null)
        {
            if (hintCoroutine != null)
                StopCoroutine(hintCoroutine);
            hintCoroutine = StartCoroutine(ShowHintCoroutine(nextHint));
        }

        hintsUsed++;
        canShowNextHint = false;

        StartCoroutine(HintCooldownCoroutine());
    }

    private IEnumerator ShowHintCoroutine(GameObject hint)
    {
        hint.SetActive(true);
        yield return new WaitForSeconds(hintDuration);
        hint.SetActive(false);
    }

    private IEnumerator HintCooldownCoroutine()
    {
        yield return new WaitForSeconds(hintCooldown);
        canShowNextHint = true;

        if ((currentHints != null && hintsUsed < currentHints.hints.Count) || (!initialHintUsed && initialHint != null))
        {
            if (hintButtonAnimator != null)
                hintButtonAnimator.SetBool(hintAvailableBool, true);
        }
    }

    private bool HasNextHint()
    {
        return currentHints != null && hintsUsed < currentHints.hints.Count;
    }

    private void HideAllHints()
    {
        if (initialHint != null)
            initialHint.SetActive(false);

        foreach (var e in eventHintsList)
        {
            foreach (var hint in e.hints)
                if (hint != null)
                    hint.SetActive(false);
        }
    }

    [System.Serializable]
    public class EventHints
    {
        public EventManager.GameEvent eventType;
        public List<GameObject> hints; // Assign the actual text objects for this event
    }
}
