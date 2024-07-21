using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutsceneManager : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(EndCutScene());
    }
    IEnumerator EndCutScene()
    {
        yield return new WaitForSeconds(17);
        SceneManager.LoadScene("MainMenu");
    }
}

