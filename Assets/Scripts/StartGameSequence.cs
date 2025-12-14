using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameSequence : MonoBehaviour
{
    [Header("Images")]
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;

    [Header("Narration Audio Sources")]
    public AudioSource audio1; // Narration for image 1
    public AudioSource audio2; // Narration for image 2

    [Header("Scene Settings")]
    public string mainSceneName = "MainEscapeRoom";
    public float lastImageDuration = 3f; // Duration for image 3

    public void OnStartButtonClicked()
    {
        StartCoroutine(ShowImagesAndLoadScene());
    }

    private IEnumerator ShowImagesAndLoadScene()
    {
        // Image 1 + Narration
        image1.SetActive(true);
        if (audio1 != null && audio1.clip != null)
        {
            audio1.Play();
            yield return new WaitForSeconds(audio1.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        image1.SetActive(false);
        if (audio1 != null) audio1.Stop();

        // Image 2 + Narration
        image2.SetActive(true);
        if (audio2 != null && audio2.clip != null)
        {
            audio2.Play();
            yield return new WaitForSeconds(audio2.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        image2.SetActive(false);
        if (audio2 != null) audio2.Stop();

        //Image 3 (No Narration, Fixed Duration)
        image3.SetActive(true);
        yield return new WaitForSeconds(lastImageDuration);
        image3.SetActive(false);

        // Load main scene
        SceneManager.LoadScene(mainSceneName);
    }
}
// This code has been inspired by Copilot and ChatGPT.

