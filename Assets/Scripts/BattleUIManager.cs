using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance { get; private set; }
    public Button attackButton;
    public Button skillButton;
    // public Button itemButton;

    public GameObject battleCanvas;

    public Character enemy, player;

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

    public void SetCharacter(Character player, Character enemy)
    {
        this.player = player;
    }
    public void OnAttackButtonClicked()
    {
        if (player != null)
        {
            player.BasicAttack(enemy);
            Debug.Log("Ich habe AttackButton geklickt");
        }
    }

    public void OnSkillButtonClicked()
    {
        if (player != null)
        {
            Debug.Log("Ich habe SkillButton geklickt");
        }
    }
    public void SetCanvasActive(bool isActive)
    {
        battleCanvas.SetActive(isActive);
    }
}
