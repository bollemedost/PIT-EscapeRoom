using UnityEngine;

public class Chest : MonoBehaviour
{
    public void OpenChest()
    {
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChestOpened);
    }
}
