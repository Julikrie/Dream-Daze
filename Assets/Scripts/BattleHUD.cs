using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : MonoBehaviour
{
    public Slider healthSliderPlayer;
    public Slider healthSliderEnemy;
    public Slider manaSliderPlayer;
    public Slider manaSliderEnemy;

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
        this.player.spentMana += UpdateManaSlider;
        this.enemy.spentMana += UpdateManaSlider;

        skillsContainer.SetActive(false);
        showSkillContainer = false;

        SetHealth(this.player, healthSliderPlayer);
        SetHealth(this.enemy, healthSliderEnemy);
        SetMana(this.player, manaSliderPlayer);
        SetMana(this.enemy, manaSliderEnemy);

        for (int i = 0; i < abilityButtons.Length; i++)
        {
            abilityButtons[i].onClick.RemoveAllListeners();

            if (i < this.player.characterAttributes.abilities.Count)
            {
                TextMeshProUGUI buttonText = abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = this.player.characterAttributes.abilities[i].Name;

                // Need this because i is not remembered in Listener
                int abilityIndex = i;

                abilityButtons[i].onClick.AddListener(() =>
                {
                    player.characterAttributes.abilities[abilityIndex].Use(this.player, this.enemy);
                    BattleManager.Instance.StartEnemyTurn();
                    DisableButtonIfManaTooLow();
                });
            }
            else
            {
                TextMeshProUGUI buttonText = abilityButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "?";
            }
        }
        DisableButtonIfManaTooLow();
    }

    public void OnBasicAttackButton()
    {
        player.BasicAttack(enemy);
        BattleManager.Instance.StartEnemyTurn();
    }

    public void OnSkillsButton()
    {
        showSkillContainer = !showSkillContainer;

        if (showSkillContainer)
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

    public void SetSkillsContainer(bool isActive)
    {
        showSkillContainer = isActive;
        skillsContainer.SetActive(isActive);
    }
    private void DisableButtonIfManaTooLow()
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (i < player.characterAttributes.abilities.Count)
            {
                Character currentCharacter = player;
                int currentIndex = i;
                abilityButtons[i].interactable = currentCharacter.characterAttributes.currentMana >= currentCharacter.characterAttributes.abilities[currentIndex].ManaCost;
            }
        }
    }

    private void SetHealth(Character character, Slider slider)
    {
        CharacterAttributes characterAttributes = character.characterAttributes;
        slider.maxValue = characterAttributes.maxHealth;
        slider.value = characterAttributes.currentHealth;
    }
    private void SetMana(Character character, Slider slider)
    {
        CharacterAttributes characterAttributes = character.characterAttributes;
        slider.maxValue = characterAttributes.maxMana;
        slider.value = characterAttributes.currentMana;
    }
    private void UpdateSlider(Character character)
    {
        if (character == this.player)
        {
            healthSliderPlayer.value = this.player.characterAttributes.currentHealth;
        }
        else
        {
            healthSliderEnemy.value = this.enemy.characterAttributes.currentHealth;
        }
    }

    private void UpdateManaSlider(Character character)
    {
        if (character == this.player)
        {
            manaSliderPlayer.value = this.player.characterAttributes.currentMana;
        }
        else
        {
            manaSliderEnemy.value = this.enemy.characterAttributes.currentMana;
        }
    }
}
