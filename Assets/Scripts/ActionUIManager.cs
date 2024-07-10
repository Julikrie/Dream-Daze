using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    public static ActionUIManager Instance { get; private set; }
    public Button moveButton;
    public Button attackButton;
    public Button waitButton;

    public GameObject actionCanvas;

    public Character character;

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
            SetCanvasActive(false);
            character.characterStateMachine.ChangeState(CharacterState.Move);
        }
    }

    public void OnAttackButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            Debug.Log("Ich habe Attack geklickt");
            SetCanvasActive(false);
            character.characterStateMachine.ChangeState(CharacterState.Attack);
        }
    }

    public void OnWaitButtonClicked()
    {
        if (character != null && character.characterStateMachine != null)
        {
            character.characterStateMachine.ChangeState(CharacterState.Wait);
            waitButton.interactable = false;
        }
    }

    public void DisableMoveButton()
    {
        moveButton.interactable = false;
    }

    public void DisableAttackButton()
    {
        attackButton.interactable = false;
    }
    public void ResetButtons()
    {
        moveButton.interactable = true;
        attackButton.interactable = true;
        waitButton.interactable = true;
    }
    void CharacterFinished()
    {
        ResetButtons();
    }

    public void SetCanvasActive(bool isActive)
    {
        if (actionCanvas != null)
        {
            actionCanvas.SetActive(isActive);
        }
    }

    public void ToggleCanvasOnClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Ich klicke rechtsklick");

            SetCanvasActive(true);
            TileSelector.Instance.ClearAttackMarkers();
            TileSelector.Instance.ClearMovementMarkers();
        }
    }
    public void SetPause(bool paused)
    {
        actionCanvas.SetActive(!paused);
    }
}

