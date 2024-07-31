using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }
    public List<Character> playerTeam;
    public List<Character> enemyTeam;

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
    public bool SameTeam(Character currentCharacter, Character otherCharacter)
    {
        if (playerTeam.Contains(currentCharacter) && playerTeam.Contains(otherCharacter))
        {
            return true;
        }
        if (enemyTeam.Contains(currentCharacter) && enemyTeam.Contains(otherCharacter))
        {
            return true;
        }
        return false;
    }

    public bool IsEnemyCharacter(Character currentCharacter)
    {
        return enemyTeam.Contains(currentCharacter);
    }

    public bool IsPlayerCharacter(Character currentCharacter)
    {
        return playerTeam.Contains(currentCharacter);
    }

    public void RemoveCharacter(Character character)
    {
        if( IsEnemyCharacter(character))
        {
            enemyTeam.Remove(character);
        }
        else
        {
            playerTeam.Remove(character);
        }
    }

}
