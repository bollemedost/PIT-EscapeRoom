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
    public AudioSource audio3; // Narration for image 3

    [Header("Scene Settings")]
    public string mainSceneName = "MainEscapeRoom";

    public void OnStartButtonClicked()
    {
        StartCoroutine(ShowImagesAndLoadScene());
    }

    private IEnumerator ShowImagesAndLoadScene()
    {
        // --- Image 1 + Narration ---
        image1.SetActive(true);
        if (audio1 != null && audio1.clip != null)
        {
            audio1.Play();
            yield return new WaitForSeconds(audio1.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(3f); // fallback if no audio
        }
        image1.SetActive(false);
        if (audio1 != null) audio1.Stop();

        // --- Image 2 + Narration ---
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

        // --- Image 3 + Narration ---
        image3.SetActive(true);
        if (audio3 != null && audio3.clip != null)
        {
            audio3.Play();
            yield return new WaitForSeconds(audio3.clip.length);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        image3.SetActive(false);
        if (audio3 != null) audio3.Stop();

        // --- Load main scene ---
        SceneManager.LoadScene(mainSceneName);
    }
}
