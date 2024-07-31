using UnityEngine;

public enum CharacterType
{
    Rock,
    Paper,
    Scissors
}

public class RockPaperScissor : MonoBehaviour

{
    private static readonly float[,] effectivenessMatrix = {
        //Rock, Paper, Scissors
        { 1.0f, 0.5f, 2.0f }, // Rock
        { 2.0f, 1.0f, 0.5f }, // Paper
        { 0.5f, 2.0f, 1.0f }  // Scissors
    };

    public static float GetEffectiveness(Character attacker, Character defender)
    {
        return effectivenessMatrix[(int)attacker.characterAttributes.characterType, (int)defender.characterAttributes.characterType];
    }
}



