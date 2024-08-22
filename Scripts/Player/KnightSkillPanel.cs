using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnightSkillPanel : MonoBehaviour
{
    public Button shieldBashButton;
    public Button heavenlyBlessingButton;
    public Button impregnableButton;

    public TextMeshProUGUI shieldBashLevelText;
    public TextMeshProUGUI heavenlyBlessingLevelText;
    public TextMeshProUGUI impregnableLevelText;

    private Knight currentKnight;

    private void Awake()
    {
        //shieldBashButton.onClick.AddListener(() => LevelUpSkill(currentKnight.shieldBash));
        //heavenlyBlessingButton.onClick.AddListener(() => LevelUpSkill(currentKnight.heavenlyBlessing));
        //impregnableButton.onClick.AddListener(() => LevelUpSkill(currentKnight.impregnable));
    }

    private void Start()
    {
        shieldBashButton.onClick.AddListener(() => LevelUpSkill(currentKnight.shieldBash));
        heavenlyBlessingButton.onClick.AddListener(() => LevelUpSkill(currentKnight.heavenlyBlessing));
        impregnableButton.onClick.AddListener(() => LevelUpSkill(currentKnight.impregnable));
    }

    public void Init()
    {
        //shieldBashButton.onClick.AddListener(() => LevelUpSkill(currentKnight.shieldBash));
        //heavenlyBlessingButton.onClick.AddListener(() => LevelUpSkill(currentKnight.heavenlyBlessing));
        //impregnableButton.onClick.AddListener(() => LevelUpSkill(currentKnight.impregnable));
    }

    public void SetCurrentKnight(Knight knight)
    {
        
        currentKnight = knight;
        Init();

        if (currentKnight == null)
        {
            Debug.LogError("SetCurrentKnight was called with a null knight.");
        }
        else
        {
            Debug.Log($"SetCurrentKnight called with knight: {currentKnight.name}");
            Debug.Log($"Knight has {currentKnight.info.skills.Count} skills:");

            foreach (var skill in currentKnight.info.skills)
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
        shieldBashLevelText.text = $"Lv.{currentKnight.shieldBash.Level}";
        heavenlyBlessingLevelText.text = $"Lv.{currentKnight.heavenlyBlessing.Level}";
        impregnableLevelText.text = $"Lv.{currentKnight.impregnable.Level}";
    }
}