using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Text levelText;
    public Image skillIcon;
    public GameObject lockIcon;
    public GameObject maxLevelIcon;

    private Skill skill;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (skill != null)
        {
            levelText.text = skill.IsUnlocked ? skill.Level.ToString() : "";

            // 스킬 아이콘 설정 (Resources 폴더에서 아이콘을 로드한다고 가정)
            string iconPath = "SkillIcons/" + skill.Name; // 스킬 이름에 따른 아이콘 경로
            Sprite skillSprite = Resources.Load<Sprite>(iconPath);
            if (skillSprite != null)
            {
                skillIcon.sprite = skillSprite;
            }
            else
            {
                Debug.LogWarning("Skill icon not found for: " + skill.Name);
            }

            lockIcon.SetActive(!skill.IsUnlocked);
            maxLevelIcon.SetActive(skill.IsUnlocked && skill.Level == skill.MaxLevel);
        }
        else
        {
            Debug.LogError("Skill is not set for this button.");
        }
    }

    public void OnClick()
    {
        if (skill != null && skill.IsUnlocked)
        {
            // 플레이어의 LevelUpSkill 메서드 호출
            // 예: Player.Instance.LevelUpSkill(skill.Name);
            Debug.Log("Attempting to level up skill: " + skill.Name);
        }
        else
        {
            Debug.Log("Skill is locked or not set.");
        }
    }
}