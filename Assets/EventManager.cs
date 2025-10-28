using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

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
        if (triggeredEvents.Contains(newEvent)) return;

        // Check if prerequisites are met before triggering
        if (CanTriggerEvent(newEvent))
        {
            triggeredEvents.Add(newEvent);
            Debug.Log($"Event triggered: {newEvent}");
            HandleEventLogic(newEvent);
        }
        else
        {
            Debug.Log($"Cannot trigger {newEvent} yet – prerequisites not met.");
        }
    }

    private bool CanTriggerEvent(GameEvent newEvent)
    {
        switch (newEvent)
        {
            case GameEvent.WandPickedUp:
                return triggeredEvents.Contains(GameEvent.ChestOpened);
            case GameEvent.RecipePickedUp:
                return triggeredEvents.Contains(GameEvent.WandPickedUp);
            case GameEvent.CorrectIngredientAdded:
                return triggeredEvents.Contains(GameEvent.RecipePickedUp);
            case GameEvent.WordCompleted:
                return triggeredEvents.Contains(GameEvent.CorrectIngredientAdded);
            case GameEvent.ChessKingMoved:
                return triggeredEvents.Contains(GameEvent.WordCompleted);
            default:
                return true; // first event or independent ones
        }
    }

    private void HandleEventLogic(GameEvent newEvent)
    {
        switch (newEvent)
        {
            case GameEvent.ChestOpened:
                // Maybe play sound or highlight the wand
                break;
            case GameEvent.WandPickedUp:
                // Allow picking up ingredients
                break;
            case GameEvent.RecipePickedUp:
                // Show recipe UI
                break;
            case GameEvent.CorrectIngredientAdded:
                // Add letter to collection
                CheckIfWordIsComplete();
                break;
            case GameEvent.WrongIngredientAdded:
                // Reject and spit out
                break;
            case GameEvent.WordCompleted:
                // Enable chess king movement
                break;
            case GameEvent.ChessKingMoved:
                // Trigger next puzzle or scene end
                break;
        }
    }

    private void CheckIfWordIsComplete()
    {
        // Example: if player has all letters K, I, N, G
        // (you can track this with your own logic)
        bool allLettersCollected = true; 
        if (allLettersCollected)
            TriggerEvent(GameEvent.WordCompleted);
    }
}
