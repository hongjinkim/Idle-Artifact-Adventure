using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoSingleton<MainUIManager>
{
    [Header("Main Screens")]
    public MainScreen _campScreen;
    public MainScreen _exploreScreen;
    public MainScreen _battleScreen;
    public MainScreen _heroScreen;
    public MainScreen _inventoryScreen;

    [Header("Toolbars")]
    public BaseScreen _playerInfoBar;
    public BaseScreen _menuBar;

    [Header("PopUp")]
    public PopUp DailyStorePopUp;
    public PopUp PortalPopUp;
    public PopUp ForgePopUp;
    public PopUp ItemInfoPopUp;
    public PopUp DungeonThemePopUp;
    public PopUp PotionPopUp;
    public PopUp SettingsPopUp;

    List<MainScreen> _allMainScreens = new List<MainScreen>();

    private Camera mainCamera;


    void OnEnable()
    {
        SetupScreens();
    }

    void Start()
    {

        Screen.SetResolution(570, 960, false);


        Time.timeScale = 1f;
        mainCamera = Camera.main;
        ShowHomeScreen();
    }

    void SetupScreens()
    {
        if (_campScreen != null)
            _allMainScreens.Add(_campScreen);

        if (_exploreScreen != null)
            _allMainScreens.Add(_exploreScreen);

        if (_battleScreen != null)
            _allMainScreens.Add(_battleScreen);

        if (_heroScreen != null)
            _allMainScreens.Add(_heroScreen);

        if (_inventoryScreen != null)
            _allMainScreens.Add(_inventoryScreen);
    }
    public void ShowSettingsPopup()
    {
        SettingsPopUp.ShowScreen();
    }
    // shows one main screen at a time
    void ShowMainScreen(MainScreen screen)
    {
        foreach (MainScreen m in _allMainScreens)
        {
            if (m == screen)
            {
                m?.ShowScreen();
            }
            else
            {
                m?.HideScreen();
            }
        }
    }

    // methods to toggle screens on/off

    public void ShowHomeScreen()
    {
        ShowMainScreen(_campScreen);
        ResetCameraPos();
    }


    public void ShowCampScreen()
    {
        ShowMainScreen(_campScreen);
        ResetCameraPos();
    }

    public void ShowExploreScreen()
    {
        ShowMainScreen(_exploreScreen);
        ResetCameraPos();
    }

    public void ShowBattleScreen()
    {
        ShowMainScreen(_battleScreen);
        ChangeCameraPos(DungeonManager.instance._selectDungeon.transform.position);
    }

    public void ShowHeroScreen()
    {
        ShowMainScreen(_heroScreen);
        ResetCameraPos();
    }

    public void ShowInventoryScreen()
    {
        ShowMainScreen(_inventoryScreen);
        ResetCameraPos();
    }

    public void TogglePlayerInfo(bool onOff)
    {
        if (onOff)
            _playerInfoBar.ShowScreen();
        else
            _playerInfoBar.HideScreen();
    }

    public void ToggleMenuBar(bool onOff)
    {
        if (onOff)
            _menuBar.ShowScreen();
        else
            _menuBar.HideScreen();
    }

    public void ResetCameraPos()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
    public void ChangeCameraPos(Vector3 position)
    {
        mainCamera.transform.position = new Vector3(position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }
}
