using UnityEngine;

public class WandPickup : MonoBehaviour
{
    public void OnPickup()
    {
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.WandPickedUp);
    }
}
