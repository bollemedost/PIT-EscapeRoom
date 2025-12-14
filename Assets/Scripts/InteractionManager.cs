using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject image1; // First explanation image
    public float imageDuration = 3f; // Duration each image is shown

    public void OnInteractionButtonClicked()
    {
        StartCoroutine(ShowImagesAndLoadScene());
    }

    private IEnumerator ShowImagesAndLoadScene()
    {
        // Show first image
        image1.SetActive(true);
        yield return new WaitForSeconds(imageDuration);
        image1.SetActive(false);
    }
}
// This code has been inspired by Copilot and ChatGPT.

