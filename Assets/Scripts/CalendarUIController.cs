using UnityEngine;

public class CalendarUIController : MonoBehaviour
{
    [Header("References")]
    public GameObject calendarCanvas; // Assign world-space Canvas

    [Header("Settings")]
    public KeyCode toggleKey = KeyCode.C; // Press 'C' to toggle the calendar

    private bool isCanvasVisible = false;

    void Start()
    {
        if (calendarCanvas != null)
            calendarCanvas.SetActive(false); // Hide at the start
    }

    void Update()
    {
        // Keyboard input for testing in scene view and not VR
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCanvas();
        }
    }

    public void ToggleCanvas()
    {
        if (calendarCanvas == null) return;

        isCanvasVisible = !isCanvasVisible;
        calendarCanvas.SetActive(isCanvasVisible);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand") || other.CompareTag("VRController"))
        {
            ToggleCanvas();
        }
    }
}
// This code has been inspired by Copilot and ChatGPT.

