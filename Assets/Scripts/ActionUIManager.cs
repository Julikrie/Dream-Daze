using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public Button moveButton;
    public Button attackButton;
    public Button waitButton;

    public Character character;

    private void Start()
    {
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
        character.characterStateMachine.Finished += CharacterFinished;

    }
    public void OnMoveButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Move geklickt");
            character.characterStateMachine.ChangeState(CharacterState.Move);
            moveButton.interactable = false;
        }
    }

    public void OnAttackButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Attack geklickt");
            character.characterStateMachine.ChangeState(CharacterState.Attack);
            attackButton.interactable = false;
        }
    }

    public void OnWaitButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Wait geklickt");
            character.characterStateMachine.ChangeState(CharacterState.Wait);
            waitButton.interactable = false;
        }
    }
    void CharacterFinished()
    {
        moveButton.interactable = true;
        attackButton.interactable = true;
        waitButton.interactable = true;
    }
}
