using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WandPickup : MonoBehaviour
{
    private bool pickedUp = false;

    // VR: called automatically when grabbed
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        TryPickup();
    }

    // Non-VR: press P to simulate pickup
    void Update()
    {
        if (!pickedUp && Input.GetKeyDown(KeyCode.P))
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (pickedUp) return;

        pickedUp = true;
        Debug.Log("✅ Wand picked up!");

        // Trigger your event
        if (EventManager.Instance != null)
            EventManager.Instance.TriggerEvent(EventManager.GameEvent.WandPickedUp);
    }
}
