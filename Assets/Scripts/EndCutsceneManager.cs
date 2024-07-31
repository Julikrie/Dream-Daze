using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutsceneManager : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = false;

        StartCoroutine(EndCutScene());
    }
    IEnumerator EndCutScene()
    {
        yield return new WaitForSeconds(17);
        SceneManager.LoadScene("MainMenu");
    }
}

