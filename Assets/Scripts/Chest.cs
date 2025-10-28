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
        // Optional editor testing
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowCodeCanvas();
        }
    }

    // Only show the code canvas, don't open chest here
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

    // This method is now only called **when the code is correct**
    public void OpenChest()
    {
        if (animator != null)
            animator.SetTrigger("Open");
    }
}
