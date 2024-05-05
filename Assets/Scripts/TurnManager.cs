using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public List<Character> characters;
    private int currentCharacterIndex;
    void Start()
    {
        foreach (Character character in characters) 
        {
            character.movementStateMachine.Finished += movementFinished;
        }
        currentCharacterIndex = 0;
        setActiveCharacter(currentCharacterIndex); 
    }
    private void movementFinished()
    {
        NextTurn();
    }
    private void setActiveCharacter(int index)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].ToggleMovement(false);
        }
        characters[index].ToggleMovement(true);
    }
    public void NextTurn()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        setActiveCharacter(currentCharacterIndex);
    }

}
