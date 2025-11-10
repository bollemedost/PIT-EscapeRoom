using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public string mainSceneName = "MenuScene"; // Name of the main game scene

    public void OnMenuButtonClicked()
    {
        // Load main scene
        SceneManager.LoadScene(mainSceneName);
    }

}
