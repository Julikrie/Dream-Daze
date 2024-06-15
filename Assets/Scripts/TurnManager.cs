using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(TeamManager.Instance.playerTeam.Count == 0)
        {
            Debug.Log("You LOST");
        }
        else if(TeamManager.Instance.enemyTeam.Count == 0) 
        {
            Debug.Log("You WIN");
        }
        else
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
            Debug.Log("Ich bin " + currentCharacterIndex);
            setActiveCharacter(currentCharacterIndex);
        }
    }

    public void RemoveCharacter(Character character)
    {
        NextTurn();
        characters.Remove(character);
    }

}

