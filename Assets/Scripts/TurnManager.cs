using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public List<Character> characters;
    public ActionUIManager actionUIManager;
    private int currentCharacterIndex;

    // Set next Character active if turn finished

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
    void Start()
    {
        foreach (Character character in characters)
        {
            character.characterStateMachine.Finished += characterFinished;
        }
        Debug.Log("Ich bin " + currentCharacterIndex);
        currentCharacterIndex = 0;
        setActiveCharacter(currentCharacterIndex);
    }
    private void characterFinished()
    {

        NextTurn();

    }
    private void setActiveCharacter(int index)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].ToggleCharacter(false);
        }
        characters[index].ToggleCharacter(true);
        ActionUIManager.Instance.ResetButtons();
        ActionUIManager.Instance.SetCharacter(characters[index]);

        if (TeamManager.Instance.IsEnemyCharacter(characters[index]))
        {
            ActionUIManager.Instance.SetCanvasActive(false);
        }
        else
        {
            ActionUIManager.Instance.SetCanvasActive(true);
        }
    }
    public void NextTurn()
    {

        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        Debug.Log("Ich bin " + currentCharacterIndex);
        setActiveCharacter(currentCharacterIndex);
    }

    public void RemoveCharacter(Character character)
    {
        characters.Remove(character);

        if (TeamManager.Instance.playerTeam.Count == 0)
        {
            Debug.Log("You LOST");
            StartCoroutine(LoadSceneWithDelay("FirstBattle", 2f)); // 2 second delay
            ActionUIManager.Instance.SetCanvasActive(false);
        }
        else if (TeamManager.Instance.enemyTeam.Count == 0)
        {
            Debug.Log("You WIN");
            StartCoroutine(LoadSceneWithDelay("FirstBattle", 2f)); // 2 second delay
            ActionUIManager.Instance.SetCanvasActive(false);
        }
        else
        {
            currentCharacterIndex = currentCharacterIndex % characters.Count;
            setActiveCharacter(currentCharacterIndex);
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
