using System;
using System.Collections;
using UnityEngine;
public enum BattleState
{
    Start, PlayerTurn, EnemyTurn, End
}
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public BattleHUD battleHUD;
    public GameObject playerImage;
    public GameObject enemyImage;
    public bool IsBattleOngoing { get; private set; }
    private Action onBattleOver;
    private Vector2 playerOldPosition;
    private Vector2 enemyOldPosition;
    private GameObject player;
    private GameObject enemy;
    private bool playerStarts;
    BattleState currentState = BattleState.Start;

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
    public void ChangeState(BattleState state)
    {
        currentState = state;
    }
    IEnumerator PlayerTurn()
    {
        battleHUD.SetButtonsContainer(true);
        //If player dies battle ends and character will be removed
        if (this.player.GetComponent<Character>().isDead)
        {
            ChangeState(BattleState.End);
            EndBattle(this.player);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator EnemyTurn()
    {
        battleHUD.SetButtonsContainer(false);
        battleHUD.SetSkillsContainer(false);

        Character enemyCharacter = this.enemy.GetComponent<Character>();
        Character playerCharacter = this.player.GetComponent<Character>();

        //If enemy dies battle ends and character will be removed
        yield return new WaitForSeconds(2f);
        if (this.enemy.GetComponent<Character>().isDead)
        {
            ChangeState(BattleState.End);
            EndBattle(this.enemy);
        }
        else
        {
            enemyCharacter.BasicAttack(playerCharacter);
            yield return new WaitForSeconds(2f);
            ChangeState(BattleState.PlayerTurn);
            StartCoroutine(PlayerTurn());
        }
    }
    public void InitiateBattle(GameObject attacker, GameObject defender, Action onBattleOver = null)
    {
        ActionUIManager.Instance.SetPause(true);
        IsBattleOngoing = true;
        playerStarts = TeamManager.Instance.IsPlayerCharacter(attacker.GetComponent<Character>());
        player = TeamManager.Instance.IsPlayerCharacter(attacker.GetComponent<Character>()) ? attacker : defender;
        enemy = TeamManager.Instance.IsPlayerCharacter(defender.GetComponent<Character>()) ? attacker : defender;
        battleHUD.AssignPlayerInformation(player.GetComponent<Character>(), enemy.GetComponent<Character>());
        playerOldPosition = player.transform.position;
        enemyOldPosition = enemy.transform.position;
        PositionCharactersForBattle(attacker);
        PositionCharactersForBattle(defender);
        battleHUD.SetCanvasActive(true);
        DetermineStartCharacter();
        this.onBattleOver = onBattleOver;
    }
    public void StartEnemyTurn()
    {   
        ChangeState(BattleState.EnemyTurn);
        StartCoroutine(EnemyTurn());
    }
    private void EndBattle(GameObject defeatedCharacter = null)
    {
        IsBattleOngoing = false;
        battleHUD.SetCanvasActive(false);
        ReturnCharactersFromBattle(player, playerOldPosition);
        ReturnCharactersFromBattle(enemy, enemyOldPosition);

        ActionUIManager.Instance.SetPause(false);
        onBattleOver?.Invoke();
        onBattleOver = null;
        if (defeatedCharacter != null)
        {
            Destroy(defeatedCharacter);
        }
    }
    private void DetermineStartCharacter()
    {
        if (playerStarts)
        {
            ChangeState(BattleState.PlayerTurn);
            StartCoroutine(PlayerTurn());
        }
        else
        {
            ChangeState(BattleState.EnemyTurn);
            StartCoroutine(EnemyTurn());
        }
    }
    private void PositionCharactersForBattle(GameObject currentCharacter)
    {
        Vector2 characterPosition;
        if (TeamManager.Instance.IsPlayerCharacter(currentCharacter.GetComponent<Character>()))
        {
            characterPosition = new Vector2(-4.2f, 0.4f);
            currentCharacter.transform.localScale = new Vector3(2, 2, 1);
        }
        else
        {
            characterPosition = new Vector2(5.4f, 0.3f);
            currentCharacter.transform.localScale = new Vector3(-2, 2, 1);

        }
        RectTransform characterTransform = currentCharacter.GetComponent<RectTransform>();
        characterTransform.anchoredPosition = characterPosition;
        SpriteRenderer characterLayer = currentCharacter.GetComponent<SpriteRenderer>();
        characterLayer.sortingOrder = 1;
    }
    private void ReturnCharactersFromBattle(GameObject currentCharacter, Vector2 oldPosition)
    {
        currentCharacter.transform.position = oldPosition;
        currentCharacter.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer characterLayer = currentCharacter.GetComponent<SpriteRenderer>();
        characterLayer.sortingOrder = 0;
    }
}
