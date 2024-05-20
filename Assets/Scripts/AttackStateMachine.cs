using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum AttackState
{
    Selection,
    Attack,
    Finished,
    Idle
}

public class AttackStateMachine
{
    public event Action Finished;

    private AttackState currentState;
    private bool isEnabled;
    private Character character;
    private List<GridCell> attackableCells;


    public AttackStateMachine(Character character)
    {
        isEnabled = false;
        this.character = character;
        this.attackableCells = new List<GridCell>();    
    }

    // Update is called once per frame
    public void Update()
    {
        if (isEnabled) 
        { 
            switch (currentState)
            {
                case AttackState.Selection:
                    Debug.Log("Ich bin in der Attack-Selektion");
                    attackableCells = TileSelector.Instance.HighlightAttackRange(character.transform.position, character.attackRange);
                    break;
                case AttackState.Attack:
                    break;
                case AttackState.Finished:
                    break;
                case AttackState.Idle:
                    break;
            }
        }
    }
    public void Enable(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }
}
