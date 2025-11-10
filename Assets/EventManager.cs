using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Testing Options")]
    [Tooltip("If enabled, all events can trigger freely without prerequisites (useful for testing).")]
    public bool testingMode = false;

    [Header("Scene References")]
    public GameObject youEscapedCanvas; // Assign your "You Escaped" Canvas here
    public Timer timerScript;           // ✅ Reference to the Timer script
    public GameObject leftHand;         // ✅ Player's left hand
    public GameObject rightHand;        // ✅ Player's right hand
    public GameObject timerCanvas;      // ✅ Timer Canvas (for hiding when escaped)

    [Header("Escape Sequence")]
    public float escapeCanvasDelay = 2f;

    [Header("Timer UI")]
    public TMP_Text timerText; // assign your timer text in the Inspector

    [Header("Portal")]
    [Tooltip("Assign the portal GameObject that should appear after a delay")]
    public GameObject portal;
    public float portalEnableDelay = 3f; // seconds to wait before enabling

    public enum GameEvent
    {
        ChestOpened,
        WandPickedUp,
        RecipePickedUp,
        CorrectIngredientAdded,
        WrongIngredientAdded,
        WordCompleted,
        ChessKingMoved,
        MagicStoneAppeared,
        StonePlaced
    }

    private HashSet<GameEvent> triggeredEvents = new HashSet<GameEvent>();

    private readonly Dictionary<GameEvent, GameEvent[]> prerequisites = new()
    {
        { GameEvent.WandPickedUp, new[] { GameEvent.ChestOpened } },
        { GameEvent.RecipePickedUp, new[] { GameEvent.WandPickedUp } },
        { GameEvent.CorrectIngredientAdded, new[] { GameEvent.RecipePickedUp } },
        { GameEvent.WordCompleted, new[] { GameEvent.CorrectIngredientAdded } },
        { GameEvent.ChessKingMoved, new[] { GameEvent.WordCompleted } },
        { GameEvent.MagicStoneAppeared, new[] { GameEvent.ChessKingMoved } },
        { GameEvent.StonePlaced, new[] { GameEvent.MagicStoneAppeared } }
    };

    public event Action<GameEvent> OnEventTriggered;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Hide "You Escaped" canvas initially
        if (youEscapedCanvas != null)
            youEscapedCanvas.SetActive(false);

        // Hide portal initially
        if (portal != null)
            portal.SetActive(false);
    }

    public void TriggerEvent(GameEvent newEvent)
    {
        if (triggeredEvents.Contains(newEvent))
        {
            Debug.Log($"Event {newEvent} already triggered – skipping.");
            return;
        }

        if (CanTriggerEvent(newEvent))
        {
            triggeredEvents.Add(newEvent);
            Debug.Log($"✅ Event triggered: {newEvent}");
            OnEventTriggered?.Invoke(newEvent);
            HandleEventLogic(newEvent);
        }
        else
        {
            Debug.Log($"❌ Cannot trigger {newEvent} yet – prerequisites not met.");
        }
    }

    private bool CanTriggerEvent(GameEvent newEvent)
    {
        if (testingMode) return true;

        if (prerequisites.TryGetValue(newEvent, out var requiredEvents))
        {
            foreach (var prereq in requiredEvents)
            {
                if (!triggeredEvents.Contains(prereq))
                {
                    Debug.Log($"⚠️ Missing prerequisite: {prereq} for {newEvent}");
                    return false;
                }
            }
        }

        return true;
    }

    private void HandleEventLogic(GameEvent newEvent)
    {
        switch (newEvent)
        {
            case GameEvent.ChestOpened:
                Debug.Log("The chest has been opened — you can now pick up the wand!");
                break;

            case GameEvent.WandPickedUp:
                Debug.Log("You picked up the wand — the recipe is now accessible!");
                break;

            case GameEvent.RecipePickedUp:
                Debug.Log("You picked up the recipe — time to add ingredients!");
                break;

            case GameEvent.CorrectIngredientAdded:
                Debug.Log("Correct ingredient added — keep going!");
                break;

            case GameEvent.WrongIngredientAdded:
                Debug.Log("Wrong ingredient added — try again!");
                break;

            case GameEvent.WordCompleted:
                Debug.Log("Word completed — the chess puzzle is now unlocked!");
                break;

            case GameEvent.ChessKingMoved:
                Debug.Log("The king has moved — puzzle complete!");
                break;

            case GameEvent.MagicStoneAppeared:
                Debug.Log("✨ Magic stone event triggered (handled externally)!");
                break;

            case GameEvent.StonePlaced:
                Debug.Log("✨ Magic stone has been placed on the target!");
                HandleEscapeSequence();

                // Enable portal after a delay
                if (portal != null)
                    StartCoroutine(EnablePortalAfterDelay());
                break;
        }
    }

    private IEnumerator EnablePortalAfterDelay()
    {
        yield return new WaitForSeconds(portalEnableDelay);
        portal.SetActive(true);
        Debug.Log("🚪 Portal enabled!");
    }

   /// <summary>
    /// Handles the final "You Escaped" sequence, with a short delay before showing the canvas.
    /// </summary>
    private void HandleEscapeSequence()
    {
        StartCoroutine(ShowEscapeCanvasWithDelay());
    }

    private IEnumerator ShowEscapeCanvasWithDelay()
    {
        // Stop and hide the timer
        if (timerScript != null)
        {
            timerScript.enabled = false;
            Debug.Log("⏸️ Timer script disabled!");
        }

        yield return new WaitForSeconds(escapeCanvasDelay);
        // Show the "You Escaped" canvas
        if (youEscapedCanvas != null)
        {
            youEscapedCanvas.SetActive(true);
            Debug.Log("🎉 You Escaped Canvas Activated!");
        }

        // Change timer color to black
        if (timerText != null)
        {
            timerText.color = Color.black;
            Debug.Log("🕒 Timer text color set to black!");
        }

        if (timerCanvas != null)
        {
            timerCanvas.SetActive(false);
            Debug.Log("🕒 Timer canvas hidden!");
        }

        // Disable player hands
        if (leftHand != null)
        {
            leftHand.SetActive(false);
            Debug.Log("🖐️ Left hand disabled!");
        }

        if (rightHand != null)
        {
            rightHand.SetActive(false);
            Debug.Log("✋ Right hand disabled!");
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 250, 300));
        GUILayout.Label("🔹 Triggered Events:");
        foreach (var evt in triggeredEvents)
            GUILayout.Label($"- {evt}");
        GUILayout.EndArea();
    }

    public bool CanTriggerExternally(GameEvent newEvent)
    {
        return CanTriggerEvent(newEvent);
    }
}
