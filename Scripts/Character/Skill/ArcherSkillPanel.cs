using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ArcherSkillPanel : MonoBehaviour
{
    public Button penetratingArrowButton;
    public Button enhancedBowButton;
    public Button marksmanshipButton;
    public Button weaknessDetectionButton;

    public TextMeshProUGUI penetratingArrowLevelText;
    public TextMeshProUGUI enhancedBowLevelText;
    public TextMeshProUGUI marksmanshipLevelText;
    public TextMeshProUGUI weaknessDetectionLevelText;

    private Archer currentArcher;


    private void Start()
    {
        penetratingArrowButton.onClick.AddListener(() => LevelUpSkill(currentArcher.penetratingArrow));
        enhancedBowButton.onClick.AddListener(() => LevelUpSkill(currentArcher.enhancedBow));
        marksmanshipButton.onClick.AddListener(() => LevelUpSkill(currentArcher.marksmanship));
        weaknessDetectionButton.onClick.AddListener(() => LevelUpSkill(currentArcher.weaknessDetection));
    }
    public void Init()
    {
        //penetratingArrowButton.onClick.AddListener(() => LevelUpSkill(currentArcher.penetratingArrow));
        //enhancedBowButton.onClick.AddListener(() => LevelUpSkill(currentArcher.enhancedBow));
        //marksmanshipButton.onClick.AddListener(() => LevelUpSkill(currentArcher.marksmanship));
        //weaknessDetectionButton.onClick.AddListener(() => LevelUpSkill(currentArcher.weaknessDetection));
    }

    public void SetCurrentArcher(Archer archer)
    {
        
        currentArcher = archer;
        Init();


        if (currentArcher == null)
        {
            Debug.LogError("SetCurrentArcher was called with a null archer.");
        }
        else
        {
            Debug.Log($"SetCurrentArcher called with archer: {currentArcher.name}");
            Debug.Log($"Archer has {currentArcher.info.skills.Count} skills:");

            foreach (var skill in currentArcher.info.skills)
            {
                Debug.Log($"Skill: {skill.Name}, Level: {skill.Level}");
            }
        }
        UpdateSkillLevels();
    }

    private void LevelUpSkill(PlayerSkill skill)
    {
        if (currentArcher == null)
        {
            Debug.LogError("currentArcher is null. Make sure SetCurrentArcher is called before attempting to level up skills.");
            return;
        }

        if (skill == null)
        {
            Debug.LogError("Attempted to level up a null skill.");
            Debug.Log($"Current archer skills: {string.Join(", ", currentArcher.info.skills.Select(s => s.Name))}");
            return;
        }

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
        penetratingArrowLevelText.text = $"Lv.{currentArcher.penetratingArrow.Level}";
        enhancedBowLevelText.text = $"Lv.{currentArcher.enhancedBow.Level}";
        marksmanshipLevelText.text = $"Lv.{currentArcher.marksmanship.Level}";
        weaknessDetectionLevelText.text = $"Lv.{currentArcher.weaknessDetection.Level}";

       
    }
}