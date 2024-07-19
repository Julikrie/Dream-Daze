using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public GameObject unlockedPage;
    public string location;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CheckBattleEnded()
    {
        {
            if (TeamManager.Instance.playerTeam.Count == 0)
            {
                Debug.Log("You LOST");
                isGameOver = true;
                ActionUIManager.Instance.SetCanvasActive(false);
                SceneManager.LoadScene("FirstBattle");
            }
            else if (TeamManager.Instance.enemyTeam.Count == 0)
            {
                Debug.Log("You WIN");
                isGameOver = true;
                ActionUIManager.Instance.SetCanvasActive(false);
                StartCoroutine(UnlockNewSkillPage());

            }
        }
            IEnumerator UnlockNewSkillPage()
        {
            unlockedPage.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            SceneManager.LoadScene(location);
        }
    }
}
