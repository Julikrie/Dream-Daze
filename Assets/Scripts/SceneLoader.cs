using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene("FirstBattle");
    }
}
