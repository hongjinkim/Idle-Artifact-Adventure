using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPanel : MonoBehaviour
{
    public UIManager uiManager;

    public void Initialize(HeroInfo info)
    {
        uiManager.ToggleMenuBar(false);
        uiManager.TogglePlayerInfo(false);


    }

    public void OnBackButtonClicked()
    {
        transform.SetAsLastSibling();
        uiManager.ToggleMenuBar(true);
        uiManager.TogglePlayerInfo(true);
    }

    public void OnGearButtonClicked()
    {

    }
    public void OnAtrributeButtonClicked()
    {

    }
    public void OnLevelUpButtonClicked()
    {

    }
    public void OnSkillButtonClicked()
    {

    }
    public void OnTalentButtonClicked()
    {

    }
}
