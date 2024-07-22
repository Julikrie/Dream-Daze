using UnityEngine;
using UnityEngine.SceneManagement;

public class BossCutsceneManager : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(sceneName);
    }
}
