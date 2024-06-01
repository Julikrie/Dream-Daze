using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject battleCanvas;
    public GameObject playerImage;
    public GameObject enemyImage;
    private Action onBattleOver;
    public bool IsBattleOngoing { get; private set; }
    private Vector2 attackerOldPosition;
    private Vector2 defenderOldPosition;

    private GameObject attacker;
    private GameObject defender;

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

    // Battle Canvas turned off on start
    private void Start()
    {
        battleCanvas.SetActive(false);
    }

    void Update()
    {
        if (IsBattleOngoing && Input.GetMouseButtonDown(0))
        {
            EndBattle();
        }
    }


    // Teleport Player and Enemy on the Battle Canvas position
    public void InitiateBattle(GameObject attacker, GameObject defender, Action onBattleOver = null)
    {
        ActionUIManager.Instance.SetCanvasActive(false);
        TileSelector.Instance.pauseIndicator = true;

        IsBattleOngoing = true;
        this.attacker = attacker;
        this.defender = defender;
        attackerOldPosition = attacker.transform.position;
        defenderOldPosition = defender.transform.position;

        PositionCharactersForBattle(attacker);
        PositionCharactersForBattle(defender);

        battleCanvas.SetActive(true);
        Debug.Log("THEY FOUGHT");

        this.onBattleOver = onBattleOver;
    }
    private void EndBattle()
    {
        ActionUIManager.Instance.SetCanvasActive(true);
        TileSelector.Instance.pauseIndicator = false;

        IsBattleOngoing = false;
        battleCanvas.SetActive(false);
        ReturnCharactersFromBattle(attacker, attackerOldPosition);
        ReturnCharactersFromBattle(defender, defenderOldPosition);

        onBattleOver?.Invoke();
        onBattleOver = null;
    }

    private void PositionCharactersForBattle(GameObject currentCharacter)
    {
        Vector2 characterPosition;
        if (TeamManager.Instance.IsPlayerCharacter(currentCharacter.GetComponent<Character>()))
        {
            characterPosition = new Vector2(-3f, -1f);
        }
        else
        {
            characterPosition = new Vector2(3f, -1f);
        }

        RectTransform characterTransform = currentCharacter.GetComponent<RectTransform>();
        characterTransform.anchoredPosition = characterPosition;
        SpriteRenderer characterLayer = currentCharacter.GetComponent<SpriteRenderer>();
        characterLayer.sortingOrder = 1;
    }
    private void ReturnCharactersFromBattle(GameObject currentCharacter, Vector2 oldPosition)
    {
        currentCharacter.transform.position = oldPosition;
        SpriteRenderer characterLayer = currentCharacter.GetComponent<SpriteRenderer>();
        characterLayer.sortingOrder = 0;
    }
}

