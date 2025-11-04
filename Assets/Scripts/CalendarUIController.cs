using UnityEngine;

public class CalendarUIController : MonoBehaviour
{
    [Header("References")]
    public GameObject calendarCanvas; // Assign your world-space Canvas here

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.C; // Press 'C' to toggle the calendar

    private bool isCanvasVisible = false;

    void Start()
    {
        if (calendarCanvas != null)
            calendarCanvas.SetActive(false); // Hide it at the start
    }

    void Update()
    {
        // Keyboard input for testing (non-VR)
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCanvas();
        }
    }

    // You can call this from VR interaction as well
    public void ToggleCanvas()
    {
        if (calendarCanvas == null) return;

        isCanvasVisible = !isCanvasVisible;
        calendarCanvas.SetActive(isCanvasVisible);
    }

    // Optional: simple collider-based interaction for VR
    private void OnTriggerEnter(Collider other)
    {
        // Example: if hand or controller touches calendar
        if (other.CompareTag("PlayerHand") || other.CompareTag("VRController"))
        {
            ToggleCanvas();
        }
    }
}
