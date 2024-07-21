using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StartCutsceneManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EndCutScene());
    }
    IEnumerator EndCutScene()
    {
        yield return new WaitForSeconds(62);
        SceneManager.LoadScene("School 1");
    }
}

