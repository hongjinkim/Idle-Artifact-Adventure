using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : BaseScreen
{
    [Header("Buttons")]
    public Button campScreenButton;
    public Button exploreScreenButton;
    public Button battleScreenButton;
    public Button heroScreenButton;
    public Button inventoryScreenButton;

    private void Start()
    {
        campScreenButton.onClick.AddListener(_UIManager.ShowCampScreen);
        exploreScreenButton.onClick.AddListener(_UIManager.ShowExploreScreen);
        battleScreenButton.onClick.AddListener(_UIManager.ShowBattleScreen);
        heroScreenButton.onClick.AddListener(_UIManager.ShowHeroScreen);
        inventoryScreenButton.onClick.AddListener(_UIManager.ShowInventoryScreen);
    }

    
}
