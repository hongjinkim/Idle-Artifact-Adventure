using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WizardSkillPanel : MonoBehaviour
{
    public Button scorchedEarthButton;
    public Button mysticResonanceButton;
    public Button waveOfHeatButton;

    public TextMeshProUGUI scorchedEarthLevelText;
    public TextMeshProUGUI mysticResonanceLevelText;
    public TextMeshProUGUI waveOfHeatLevelText;

    private Wizard currentWizard;

    private void Start()
    {
        scorchedEarthButton.onClick.AddListener(() => LevelUpSkill(currentWizard.scorchedEarth));
        mysticResonanceButton.onClick.AddListener(() => LevelUpSkill(currentWizard.mysticResonance));
        waveOfHeatButton.onClick.AddListener(() => LevelUpSkill(currentWizard.waveOfHeat));
    }
    private void Init()
    {
        
        //scorchedEarthButton.onClick.AddListener(() => LevelUpSkill(currentWizard.scorchedEarth));
        //mysticResonanceButton.onClick.AddListener(() => LevelUpSkill(currentWizard.mysticResonance));
        //waveOfHeatButton.onClick.AddListener(() => LevelUpSkill(currentWizard.waveOfHeat));
    }

    public void SetCurrentWizard(Wizard wizard)
    {
        
        
        currentWizard = wizard;
        
        Init();
        if (currentWizard == null)
        {
            Debug.LogError("SetCurrentWizard was called with a null wizard.");
        }
        else
        {
            Debug.Log($"SetCurrentWizard called with wizard: {currentWizard.name}");
            Debug.Log($"Wizard has {currentWizard.info.skills.Count} skills:");

            foreach (var skill in currentWizard.info.skills)
            {
                Debug.Log($"Skill: {skill.Name}, Level: {skill.Level}");
            }
        }
        UpdateSkillLevels();
    }

    private void LevelUpSkill(PlayerSkill skill)
    {
        Debug.Log(skill);
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
        scorchedEarthLevelText.text = $"Lv.{currentWizard.scorchedEarth.Level}";
        mysticResonanceLevelText.text = $"Lv.{currentWizard.mysticResonance.Level}";
        waveOfHeatLevelText.text = $"Lv.{currentWizard.waveOfHeat.Level}";
    }
}