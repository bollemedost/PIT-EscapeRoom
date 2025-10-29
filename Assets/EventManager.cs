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

    [Header("Scene References")]
    public GameObject magicStone; // Assign your magic stone GameObject here

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
        { EventManager.GameEvent.StonePlaced, new[] { EventManager.GameEvent.MagicStoneAppeared } }
    };

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

        // Make sure the Magic Stone starts inactive
        if (magicStone != null)
            magicStone.SetActive(false);
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
                Debug.Log("✨ A magic stone has appeared in the room!");
                if (magicStone != null)
                    magicStone.SetActive(true);
                break;
                
            case GameEvent.StonePlaced:
                Debug.Log("✨ Magic stone has been placed on the target!");
                break;
        }
    }

    private void CheckIfWordIsComplete()
    {
        // This function is optional; you can implement logic if needed
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
