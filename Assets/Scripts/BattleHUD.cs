using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : MonoBehaviour
{
    public Slider healthSliderPlayer;
    public Slider healthSliderEnemy;
    //SPIELERNAME
    //GEGNERNAME
    public GameObject battleCanvas;
    public GameObject skillsContainer;
    public GameObject buttonsContainer;
    public Button[] abilityButtons = new Button[4];

    private bool showSkillContainer;

    Character player;
    Character enemy;

    void Start()
    {
        battleCanvas.SetActive(false);
    }
    public void AssignPlayerInformation(Character player, Character enemy)
    {
        this.player = player;
        this.enemy = enemy;
        this.player.tookDamage += UpdateSlider;
        this.enemy.tookDamage += UpdateSlider;

        skillsContainer.SetActive(false);

        Debug.Log($"Enemy has health {enemy.characterAttributes.currentHealth}");
        Debug.Log($"Player has health {player.characterAttributes.currentHealth}");
        SetHealth(player, healthSliderPlayer);
        SetHealth(enemy, healthSliderEnemy);

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < player.abilities.Count)
            {
                TextMeshProUGUI buttonText = abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = player.abilities[i].Name;
                int abilityIndex = i;
                abilityButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"Button with {player.abilities[abilityIndex].Name}, index is {abilityIndex} clicked and size of abilities is {player.abilities.Count}");
                    player.abilities[abilityIndex].Use(player, enemy);
                    BattleManager.Instance.StartEnemyTurn();
                    DisableButtonIfManaTooLow();
                });
            }
            else
            {
                Debug.Log("Character does not have enough skills to assign to button " + i);
            }
        }
        DisableButtonIfManaTooLow();
    }

    public void OnBasicAttackButton()
    {
        player.BasicAttack(enemy);
        BattleManager.Instance.StartEnemyTurn();
    }
    public void OnDefendButton()
    {
        //player.Defend();
        BattleManager.Instance.StartEnemyTurn();
    }
    public void OnSkillsButton()
    {
        showSkillContainer = !showSkillContainer;

        if(showSkillContainer)
        {
            skillsContainer.SetActive(true);
        }
        else
        {
        skillsContainer.SetActive(false); 
        }
    }

    public void SetCanvasActive(bool isActive)
    {
        battleCanvas.SetActive(isActive);
    }

    public void SetButtonsContainer(bool isActive)
    {
        buttonsContainer.SetActive(isActive);
    }
    private void DisableButtonIfManaTooLow()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < player.abilities.Count)
            {
                Character currentCharacter = player; 
                int currentIndex = i; 
                abilityButtons[i].interactable = currentCharacter.characterAttributes.currentMana >= currentCharacter.abilities[currentIndex].manaCost;
            }
        }
    }

    private void SetHealth(Character character, Slider slider)
    {
        CharacterAttributes characterAttributes = character.characterAttributes;
        slider.maxValue = characterAttributes.maxHealth;
        slider.value = characterAttributes.currentHealth;
    }
    private void UpdateSlider(Character character)
    {
        if(character == this.player)
        {
            healthSliderPlayer.value = this.player.characterAttributes.currentHealth;
        }
        else
        {
            healthSliderEnemy.value = this.enemy.characterAttributes.currentHealth;
        }
    }
}
