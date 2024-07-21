using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public GameObject unlockedPage;
    public string location;
    public string currentBattle;
    public GameObject pauseMenu;


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

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        ActivatePauseMenu();
    }

    public void CheckBattleEnded()
    {
        {
            if (TeamManager.Instance.playerTeam.Count == 0)
            {
                Debug.Log("You LOST");
                isGameOver = true;
                ActionUIManager.Instance.SetCanvasActive(false);
                pauseMenu.SetActive(true);
            }
            else if (TeamManager.Instance.enemyTeam.Count == 0)
            {
                Debug.Log("You WIN");
                isGameOver = true;
                ActionUIManager.Instance.SetCanvasActive(false);
                StartCoroutine(UnlockNewSkillPage());

            }
        }
   
    }

    IEnumerator UnlockNewSkillPage()
    {
        unlockedPage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(location);
    }

    public void ActivatePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            { 
                if (pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentBattle);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
