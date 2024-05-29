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

    // Chooses character
    public void SetCharacter(Character character)
    {
        this.character = character;
        character.characterStateMachine.Finished += CharacterFinished;

    }

    // Changes to MoveState and disables Move Button
    public void OnMoveButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Move geklickt");
            character.characterStateMachine.ChangeState(CharacterState.Move);
            moveButton.interactable = false;
        }
    }

    // Changes to AttackState and disables Attack Button
    public void OnAttackButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Attack geklickt");
            character.characterStateMachine.ChangeState(CharacterState.Attack);
            attackButton.interactable = false;
        }
    }

    // Changes to WaitState/Idle and disables Wait Button
    public void OnWaitButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            character.characterStateMachine.ChangeState(CharacterState.Wait);
            waitButton.interactable = false;
        }
    }

    // Reset the Buttons for next turn
    void CharacterFinished()
    {
        moveButton.interactable = true;
        attackButton.interactable = true;
        waitButton.interactable = true;
    }
}

