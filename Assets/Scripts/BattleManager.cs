using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class BattleManager : MonoBehaviour
{
    public GameObject battleCanvas;
    public GameObject playerImage;
    public GameObject enemyImage;
    public static BattleManager Instance { get; private set; }

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

    // Teleport Player and Enemy on the Battle Canvas position
    public void InitiateBattle(GameObject attacker, GameObject enemy)
    {
        Vector2 attackOldPosition = attacker.transform.position;
        Vector2 enemyOldPosition = enemy.transform.position;

        RectTransform attackerTransform = attacker.GetComponent<RectTransform>();
        attackerTransform.anchoredPosition = new Vector2(-3f, -1f);
        SpriteRenderer attackerLayer = attacker.GetComponent<SpriteRenderer>();
        attackerLayer.sortingOrder = 1;

        RectTransform enemyTransform = enemy.GetComponent<RectTransform>();
        SpriteRenderer enemyLayer = enemy.GetComponent<SpriteRenderer>();
        enemyTransform.anchoredPosition = new Vector2(3f, -1f);
        enemyLayer.sortingOrder = 1;

        battleCanvas.SetActive(true);
    }
}
