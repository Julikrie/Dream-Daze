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
    bool playerStarts;
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

        //Wenn Spieler Dead, beenden und nach dem Kampf Player charakter löschen
        if (this.player.GetComponent<Character>().isDead)
        {
            ChangeState(BattleState.End);
            EndBattle(this.player);
        }
        else
        {
            //TODO: PlayerBuffs/Debuffs runterzählen und entfernen
            Debug.Log("PLAYER TURN");
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator EnemyTurn()
    {
        battleHUD.SetButtonsContainer(false);

        Character enemyCharacter = this.enemy.GetComponent<Character>();
        Character playerCharacter = this.player.GetComponent<Character>();

        //Wenn Enemy Dead, beenden und Player charakter löschen
        if (this.enemy.GetComponent<Character>().isDead)
        {
            ChangeState(BattleState.End);
            EndBattle(this.enemy);
        }
        else
        {
            Debug.Log("ENEMY TURN");
            yield return new WaitForSeconds(2f);
            enemyCharacter.BasicAttack(playerCharacter);
            yield return new WaitForSeconds(2f);
            //TODO: EnemyBuffs/Debuffs runterzählen und entfernen
            ChangeState(BattleState.PlayerTurn);
            StartCoroutine(PlayerTurn());
        }
    }
    public void InitiateBattle(GameObject attacker, GameObject defender, Action onBattleOver = null)
    {
        //TileSelector.Instance.SetPause(true);
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
        // TileSelector.Instance.SetPause(false);
        ActionUIManager.Instance.SetPause(false);
        onBattleOver?.Invoke();
        onBattleOver = null;
        if (defeatedCharacter != null)
        {
            Destroy(defeatedCharacter);
            //TODO: Bei Aufruf Charakter aus TeamManager, TurnManager und Grid entfernen, dann GameObject löschen
            //Destroy(defeatedCharacter); // Oder auf Board DeathAnimation abspielen dann entfernen z.b. mit defeatedCharacter.defeated()
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
