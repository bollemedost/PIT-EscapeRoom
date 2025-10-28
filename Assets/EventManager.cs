using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Header("Testing Options")]
    [Tooltip("If enabled, all events can trigger freely without prerequisites (useful for testing).")]
    public bool testingMode = true;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Trigger an event and handle logic
    public void TriggerEvent(GameEvent newEvent)
    {
        if (triggeredEvents.Contains(newEvent))
        {
            Debug.Log($"Event {newEvent} was already triggered – skipping.");
            return;
        }

        // Check if prerequisites are met before triggering
        if (CanTriggerEvent(newEvent))
        {
            triggeredEvents.Add(newEvent);
            Debug.Log($"✅ Event triggered: {newEvent}");
            HandleEventLogic(newEvent);
        }
        else
        {
            Debug.Log($"❌ Cannot trigger {newEvent} yet – prerequisites not met.");
        }
    }

    private bool CanTriggerEvent(GameEvent newEvent)
    {
        if (testingMode) return true; // allow all for testing

        switch (newEvent)
        {
            case GameEvent.WandPickedUp:
                if (!triggeredEvents.Contains(GameEvent.ChestOpened))
                {
                    Debug.Log("⚠️ Missing prerequisite: ChestOpened");
                    return false;
                }
                return true;

            case GameEvent.RecipePickedUp:
                if (!triggeredEvents.Contains(GameEvent.WandPickedUp))
                {
                    Debug.Log("⚠️ Missing prerequisite: WandPickedUp");
                    return false;
                }
                return true;

            case GameEvent.CorrectIngredientAdded:
                if (!triggeredEvents.Contains(GameEvent.RecipePickedUp))
                {
                    Debug.Log("⚠️ Missing prerequisite: RecipePickedUp");
                    return false;
                }
                return true;

            case GameEvent.WordCompleted:
                if (!triggeredEvents.Contains(GameEvent.CorrectIngredientAdded))
                {
                    Debug.Log("⚠️ Missing prerequisite: CorrectIngredientAdded");
                    return false;
                }
                return true;

            case GameEvent.ChessKingMoved:
                if (!triggeredEvents.Contains(GameEvent.WordCompleted))
                {
                    Debug.Log("⚠️ Missing prerequisite: WordCompleted");
                    return false;
                }
                return true;

            default:
                return true; // independent or first event
        }
    }

    private void HandleEventLogic(GameEvent newEvent)
    {
        switch (newEvent)
        {
            case GameEvent.ChestOpened:
                // Example: highlight wand or play sound
                break;

            case GameEvent.WandPickedUp:
                // Example: allow picking up ingredients
                break;

            case GameEvent.RecipePickedUp:
                // Example: show recipe UI
                break;

            case GameEvent.CorrectIngredientAdded:
                // Example: collect letter
                CheckIfWordIsComplete();
                break;

            case GameEvent.WrongIngredientAdded:
                // Example: reject and play sound
                break;

            case GameEvent.WordCompleted:
                // Example: unlock chess event
                break;

            case GameEvent.ChessKingMoved:
                // Example: end puzzle or trigger cutscene
                break;
        }
    }

    private void CheckIfWordIsComplete()
    {
        // Example logic — you can track this properly later
        bool allLettersCollected = true;
        if (allLettersCollected)
            TriggerEvent(GameEvent.WordCompleted);
    }
}
