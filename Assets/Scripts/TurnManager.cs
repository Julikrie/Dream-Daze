using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public GameObject unlockPage;
    public static TurnManager Instance { get; private set; }

    public List<Character> characters;
    public ActionUIManager actionUIManager;
    private int currentCharacterIndex;

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
            character.characterStateMachine.Finished += CharacterFinished;
        }
        currentCharacterIndex = 0;
        SetActiveCharacter(currentCharacterIndex);
        unlockPage.SetActive(false);
    }

    private void CharacterFinished()
    {

        NextTurn();
    }

    private void SetActiveCharacter(int index)
    {
        if (characters.Count == 0 || GameManager.Instance.isGameOver)
        {
            return;
        }

        //Skip characters marked with null in the array
        while (characters[index] == null)
        {
            index = (index + 1) % characters.Count;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] != null)
            {
                characters[i].ToggleCharacter(false);
            }
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
        if (characters.Count == 0 || GameManager.Instance.isGameOver)
        {
            return;
        }

        currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;

        //Skip characters marked with null in the array
        while (characters[currentCharacterIndex] == null)
        {
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Count;
        }

        Debug.Log($"Nächster Charakter {currentCharacterIndex}");
        SetActiveCharacter(currentCharacterIndex);
    }

    public void RemoveCharacter(Character character)
    {
        if (!GameManager.Instance.isGameOver)
        {
            int characterIndex = characters.IndexOf(character);

            if (characterIndex < 0) return;

            characters[characterIndex] = null;

            GameManager.Instance.CheckBattleEnded();


            if (GameManager.Instance.isGameOver)
            {
                return;
            }

            // If the character being removed is the current character, call characterFinished
            if (characterIndex == currentCharacterIndex)
            {
                CharacterFinished();
            }
        }
    }
}