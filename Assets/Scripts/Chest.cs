using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;

    [Header("Code Canvas Setup")]
    public GameObject codeCanvasPrefab; // assign your canvas prefab
    private GameObject spawnedCanvas;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("No Animator found on the Chest!");
    }

    void Update()
    {
        // Optional: For desktop testing only
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowCodeCanvas();
        }
    }

    // Show the code canvas (for entering code, etc.)
    public void ShowCodeCanvas()
    {
        if (spawnedCanvas != null) return;

        Transform cameraTransform = Camera.main.transform;
        spawnedCanvas = Instantiate(codeCanvasPrefab);
        spawnedCanvas.transform.SetParent(cameraTransform);

        // Position canvas in front of player
        spawnedCanvas.transform.localPosition = new Vector3(0, 0, 0.6f);
        spawnedCanvas.transform.localRotation = Quaternion.identity;
        spawnedCanvas.SetActive(true);
    }

    // Called when the player has entered the correct code or completed the puzzle
    public void OpenChest()
    {
        if (animator != null)
            animator.SetTrigger("Open");

        // ✅ Tell the event manager this event happened
        EventManager.Instance.TriggerEvent(EventManager.GameEvent.ChestOpened);
    }
}
