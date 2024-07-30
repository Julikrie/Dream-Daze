using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("StartCutScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void EnterGuide()
    {
        SceneManager.LoadScene("Guide");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
