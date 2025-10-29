using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Testing Options")]
    [Tooltip("If enabled, all events can trigger freely without prerequisites (useful for testing).")]
    public bool testingMode = false;

    public enum GameEvent
    {
        ChestOpened,
        WandPickedUp,
        RecipePickedUp,
        CorrectIngredientAdded,
        WrongIngredientAdded,
        WordCompleted,
        ChessKingMoved
    }

    private HashSet<GameEvent> triggeredEvents = new HashSet<GameEvent>();

    // Define prerequisites for event order
    private readonly Dictionary<GameEvent, GameEvent[]> prerequisites = new()
    {
        { GameEvent.WandPickedUp, new[] { GameEvent.ChestOpened } },
        { GameEvent.RecipePickedUp, new[] { GameEvent.WandPickedUp } },
        { GameEvent.CorrectIngredientAdded, new[] { GameEvent.RecipePickedUp } },
        { GameEvent.WordCompleted, new[] { GameEvent.CorrectIngredientAdded } },
        { GameEvent.ChessKingMoved, new[] { GameEvent.WordCompleted } },
    };

    // Allow other scripts to listen for events
    public event Action<GameEvent> OnEventTriggered;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        // Example logic — you can replace with your actual game actions
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
        }
    }

    private void CheckIfWordIsComplete()
    {
        // Example logic for testing — auto-triggers completion
        bool allLettersCollected = true;
        if (allLettersCollected)
            TriggerEvent(GameEvent.WordCompleted);
    }

    // For debug visibility in Inspector
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
