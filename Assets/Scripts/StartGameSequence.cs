using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameSequence : MonoBehaviour
{
    public GameObject image1;        // First explanation image
    public GameObject image2;        // Second explanation image
    public GameObject image3;        // Third explanation image
    public float imageDuration = 3f; // Duration each image is shown
    public string mainSceneName = "MainEscapeRoom"; // Name of the main game scene

    public void OnStartButtonClicked()
    {
        StartCoroutine(ShowImagesAndLoadScene());
    }

    private IEnumerator ShowImagesAndLoadScene()
    {
        // Show first image
        image1.SetActive(true);
        yield return new WaitForSeconds(imageDuration);
        image1.SetActive(false);

        // Show second image
        image2.SetActive(true);
        yield return new WaitForSeconds(imageDuration);
        image2.SetActive(false);

        // Show third image
        image3.SetActive(true);
        yield return new WaitForSeconds(imageDuration);
        image3.SetActive(false);

        // Load main scene
        SceneManager.LoadScene(mainSceneName);
    }
}
