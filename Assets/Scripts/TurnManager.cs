using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Character> characters;
    public ActionUIManager actionUIManager;
    private int currentCharacterIndex;

    // Set next Character active if turn finished
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
        actionUIManager.SetCharacter(characters[index]);
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

}

