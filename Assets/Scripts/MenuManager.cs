using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
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
