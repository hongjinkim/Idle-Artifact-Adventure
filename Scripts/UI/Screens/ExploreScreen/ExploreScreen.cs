using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreScreen : MainScreen
{
    [Header("Managers")]
    private DungeonManager dungeonManager;

    [Header("Stages")]
    public Button stage1;
    public Button stage2;
    public Button stage3;
    public Button stage4;

    protected override void Awake()
    {
        base.Awake();
        if(dungeonManager == null)
        {
            dungeonManager = DungeonManager.instance;
        }
    }

    private void Start()
    {
        stage1.onClick.AddListener(() => OpenAdventurePopup("A"));
        stage2.onClick.AddListener(() => OpenAdventurePopup("B"));
        stage3.onClick.AddListener(() => OpenAdventurePopup("C"));
        stage4.onClick.AddListener(() => OpenAdventurePopup("D"));
    }

    private void OpenAdventurePopup(string _themeCode)
    {
        dungeonManager._themeList.Clear();

        foreach (var dungeon in dungeonManager._allDungeonList)
        {
            if (dungeon._themeCode.Equals(_themeCode) && !dungeonManager._themeList.Contains(dungeon))
            {
                dungeonManager._themeList.Add(dungeon);
            }
        }

        if (MainUIManager.instance.DungeonThemePopUp == null)
        {
            dungeonManager.SelectDungeon(0);
        }


        _UIManager.DungeonThemePopUp.ShowScreen();

        //_UIManager.ExploreThemepopUp.GetComponent<ExploreThemePopup>().InitAdventurePopup(dungeonManager._themeList);



    }
}
