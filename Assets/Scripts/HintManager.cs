using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [Header("Hints Configuration")]
    public List<EventHints> eventHintsList;

    [Header("UI References")]
    public Button hintButton;
    public Animator hintButtonAnimator;
    public string hintAvailableBool = "HintAvailableBool";

    [Header("Hint Settings")]
    public float hintDuration = 3f;
    public float hintCooldown = 5f;
    public float fadeDuration = 1f; // how long fade in/out takes
    public float audioDelay = 0.3f; // ✅ new: delay before hint audio starts

    [Header("Initial Hint")]
    public GameObject initialHint;

    [Header("Owl")]
    public OwlController owl;

    private EventManager.GameEvent currentEvent;
    private int hintsUsed = 0;
    private Coroutine hintCoroutine;
    private EventHints currentHints;
    private bool canShowNextHint = true;
    private bool initialHintUsed = false;
    private bool firstEventTriggered = false;

    void Start()
    {
        EventManager.Instance.OnEventTriggered += OnEventTriggered;

        if (hintButton != null)
            hintButton.onClick.AddListener(ShowHint);

        HideAllHints();

        if (initialHint != null)
        {
            initialHint.SetActive(false);
            initialHintUsed = false;
        }

        if (hintButtonAnimator != null)
            hintButtonAnimator.SetBool(hintAvailableBool, true);

        if (owl != null)
            owl.ResetNotes();
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
            hintButton.onClick.RemoveListener(ShowHint);
    }

    private void OnEventTriggered(EventManager.GameEvent evt)
    {
        currentEvent = evt;
        hintsUsed = 0;
        firstEventTriggered = true;

        currentHints = eventHintsList.Find(e => e.eventType == currentEvent);
        HideAllHints();
        canShowNextHint = true;

        if (owl != null)
            owl.ResetNotes();

        if (initialHint != null)
            initialHint.SetActive(false);

        if (hintButtonAnimator != null)
        {
            bool hasHints = firstEventTriggered && HasNextHint();
            hintButtonAnimator.SetBool(hintAvailableBool, hasHints);
        }
    }

    public void ShowHint()
    {
        if (!canShowNextHint) return;

        // --- Initial hint ---
        if (!initialHintUsed && !firstEventTriggered && initialHint != null)
        {
            if (owl != null)
                owl.Fly();

            if (hintCoroutine != null)
                StopCoroutine(hintCoroutine);
            hintCoroutine = StartCoroutine(ShowHintCoroutine(initialHint));

            initialHintUsed = true;
            canShowNextHint = false;
            StartCoroutine(HintCooldownCoroutine());

            if (hintButtonAnimator != null)
                hintButtonAnimator.SetBool(hintAvailableBool, false);

            return;
        }

        // --- Event hints ---
        if (currentHints == null || currentHints.hints.Count == 0) return;
        if (hintsUsed >= currentHints.hints.Count) return;

        GameObject nextHint = currentHints.hints[hintsUsed];
        if (nextHint != null)
        {
            if (owl != null)
                owl.Fly();

            if (hintCoroutine != null)
                StopCoroutine(hintCoroutine);
            hintCoroutine = StartCoroutine(ShowHintCoroutine(nextHint));

            if (hintButtonAnimator != null)
                hintButtonAnimator.SetBool(hintAvailableBool, false);
        }

        hintsUsed++;
        canShowNextHint = false;
        StartCoroutine(HintCooldownCoroutine());
    }

    private IEnumerator ShowHintCoroutine(GameObject hint)
    {
        hint.SetActive(true);

        // --- Get or add CanvasGroup ---
        CanvasGroup cg = hint.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = hint.AddComponent<CanvasGroup>();

        // --- Get AudioSource (if present) ---
        AudioSource audioSource = hint.GetComponent<AudioSource>();

        // --- Fade in ---
        yield return StartCoroutine(FadeCanvasGroup(cg, 0f, 1f, fadeDuration));

        // --- Play narration after short delay ---
        if (audioSource != null && audioSource.clip != null)
        {
            yield return new WaitForSeconds(audioDelay);
            audioSource.Play();
        }

        // --- Wait for either hintDuration or clip length ---
        float waitTime = hintDuration;
        if (audioSource != null && audioSource.clip != null)
            waitTime = Mathf.Max(hintDuration, audioSource.clip.length + audioDelay); // include delay

        yield return new WaitForSeconds(waitTime);

        // --- Fade out ---
        yield return StartCoroutine(FadeCanvasGroup(cg, 1f, 0f, fadeDuration));

        // --- Stop audio ---
        if (audioSource != null)
            audioSource.Stop();

        hint.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        cg.alpha = end;
    }

    private IEnumerator HintCooldownCoroutine()
    {
        yield return new WaitForSeconds(hintCooldown);
        canShowNextHint = true;

        if (hintButtonAnimator != null && HasNextHint())
            hintButtonAnimator.SetBool(hintAvailableBool, true);
    }

    private bool HasNextHint()
    {
        return currentHints != null && hintsUsed < currentHints.hints.Count;
    }

    private void HideAllHints()
    {
        if (initialHint != null) initialHint.SetActive(false);
        foreach (var e in eventHintsList)
            foreach (var hint in e.hints)
                if (hint != null) hint.SetActive(false);
    }

    [System.Serializable]
    public class EventHints
    {
        public EventManager.GameEvent eventType;
        public List<GameObject> hints;
    }
}
