using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PriestSkillPanel : MonoBehaviour
{
    public Button purifyingLightButton;
    public Button dazzlingLightButton;
    public Button holyGraceButton;
    public Button mysticalPowerButton;

    public TextMeshProUGUI purifyingLightLevelText;
    public TextMeshProUGUI dazzlingLightLevelText;
    public TextMeshProUGUI holyGraceLevelText;
    public TextMeshProUGUI mysticalPowerLevelText;

    private Priest currentPriest;

    private void Start()
    {
        purifyingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.purifyingLight));
        dazzlingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.dazzlingLight));
        holyGraceButton.onClick.AddListener(() => LevelUpSkill(currentPriest.holyGrace));
        mysticalPowerButton.onClick.AddListener(() => LevelUpSkill(currentPriest.mysticalPower));
    }

    private void Init()
    {
        //purifyingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.purifyingLight));
        //dazzlingLightButton.onClick.AddListener(() => LevelUpSkill(currentPriest.dazzlingLight));
        //holyGraceButton.onClick.AddListener(() => LevelUpSkill(currentPriest.holyGrace));
        //mysticalPowerButton.onClick.AddListener(() => LevelUpSkill(currentPriest.mysticalPower));
    }

    public void SetCurrentPriest(Priest priest)
    {
        ///Debug.Log(priest.info.skills.Count);
        
        currentPriest = priest;
        Init();
        if (currentPriest == null)
        {
            Debug.LogError("SetCurrentPriest was called with a null priest.");
        }
        else
        {
            Debug.Log($"SetCurrentPriest called with priest: {currentPriest.name}");
            Debug.Log($"priest has {currentPriest.info.skills.Count} skills:");

            foreach (var skill in currentPriest.info.skills)
            {
                Debug.Log($"Skill: {skill.Name}, Level: {skill.Level}");
            }
        }
        UpdateSkillLevels();

    }

    private void LevelUpSkill(PlayerSkill skill)
    {
        if (skill.Level < skill.MaxLevel)
        {
            int oldLevel = skill.Level;
            skill.LevelUp();
            Debug.Log($"{skill.Name} leveled up from {oldLevel} to {skill.Level}");
            UpdateSkillLevels();
        }
        else
        {
            Debug.Log($"{skill.Name} is already at max level ({skill.MaxLevel})");
            
        }
    }

    private void UpdateSkillLevels()
    {
        purifyingLightLevelText.text = $"Lv.{currentPriest.purifyingLight.Level}";
        dazzlingLightLevelText.text = $"Lv.{currentPriest.dazzlingLight.Level}";
        holyGraceLevelText.text = $"Lv.{currentPriest.holyGrace.Level}";
        mysticalPowerLevelText.text = $"Lv.{currentPriest.mysticalPower.Level}";
    }
}