using UnityEngine;

public class GameProgressController : MonoBehaviour
{
    [Header("Interactables")]
    public GameObject chest;
    public GameObject wand;
    public GameObject recipe;
    public GameObject cauldron;
    public GameObject chess;

    void Start()
    {
        // Start with only the chest interactable active
        chest.SetActive(true);
        wand.SetActive(false);
        recipe.SetActive(false);
        cauldron.SetActive(false);
        chess.SetActive(false);

        // Subscribe to EventManager
        EventManager.Instance.OnEventTriggered += OnEventTriggered;
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered -= OnEventTriggered;
    }

    private void OnEventTriggered(EventManager.GameEvent evt)
    {
        switch (evt)
        {
            case EventManager.GameEvent.ChestOpened:
                wand.SetActive(true);      // enable wand
                break;

            case EventManager.GameEvent.WandPickedUp:
                recipe.SetActive(true);    // enable recipe
                break;

            case EventManager.GameEvent.RecipePickedUp:
            case EventManager.GameEvent.CorrectIngredientAdded:
                cauldron.SetActive(true);  // enable cauldron
                break;

            case EventManager.GameEvent.WordCompleted:
                chess.SetActive(true);     // enable chess
                break;
        }
    }
}
