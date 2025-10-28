using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public void AddIngredient(GameObject ingredient)
    {
        if (IsCorrectIngredient(ingredient))
        {
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.CorrectIngredientAdded);
        }
        else
        {
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.WrongIngredientAdded);
            RejectIngredient(ingredient);
        }
    }

    private bool IsCorrectIngredient(GameObject ingredient)
    {
        // Add your own logic, maybe tag-based check
        return ingredient.CompareTag("CorrectIngredient");
    }

    private void RejectIngredient(GameObject ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // spit out effect
        }
    }
}
