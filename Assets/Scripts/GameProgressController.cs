using UnityEngine;

public class GameProgressController : MonoBehaviour
{
    [Header("Interactables")]
    public GameObject chest;
    public GameObject wand;
    public GameObject recipe;
    public GameObject cauldron;
    public GameObject chess;
    public GameObject magicStone; // Reference to the magic stone
    public GameObject stoneTarget; // Optional: show the stone target when stone can be placed

    void Start()
    {
        // Start with only the chest interactable active
        chest.SetActive(true);
        wand.SetActive(false);
        recipe.SetActive(false);
        cauldron.SetActive(false);
        chess.SetActive(false);
        magicStone.SetActive(false);
        if (stoneTarget != null)
            stoneTarget.SetActive(false); // Stone target starts inactive

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
                wand.SetActive(true);
                break;

            case EventManager.GameEvent.WandPickedUp:
                recipe.SetActive(true);
                break;

            case EventManager.GameEvent.RecipePickedUp:
            case EventManager.GameEvent.CorrectIngredientAdded:
                cauldron.SetActive(true);
                break;

            case EventManager.GameEvent.WordCompleted:
                chess.SetActive(true);
                break;

            case EventManager.GameEvent.MagicStoneAppeared:
                magicStone.SetActive(true);
                if (stoneTarget != null)
                    stoneTarget.SetActive(true); // Show target for placing the stone
                break;

            case EventManager.GameEvent.StonePlaced:
                Debug.Log("Stone placed! You can now trigger the next event or effect.");
                // Optional: hide target or trigger next step
                if (stoneTarget != null)
                    stoneTarget.SetActive(false);
                break;
        }
    }
}
