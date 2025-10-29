using UnityEngine;

public class SubstituteObjectController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject substitute; // visible placeholder
    public GameObject realObject; // actual interactable

    [Header("Event to activate real object")]
    public EventManager.GameEvent activateEvent;

    void Start()
    {
        // Start with substitute visible and real object disabled
        if (substitute != null) substitute.SetActive(true);
        if (realObject != null) realObject.SetActive(false);

        // Subscribe to EventManager
        EventManager.Instance.OnEventTriggered += OnEventTriggered;
    }

    void OnDestroy()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.OnEventTriggered -= OnEventTriggered;
    }

    private void OnEventTriggered(EventManager.GameEvent evt)
    {
        if (evt == activateEvent)
        {
            if (realObject != null) realObject.SetActive(true);     // enable real object
            if (substitute != null) substitute.SetActive(false);    // hide substitute
        }
    }
}
