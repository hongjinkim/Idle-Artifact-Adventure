using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampScreen : MainScreen
{
    [Header("Buttons")]
    public Button portalButton;
    public Button dailyStoreButton;
    public Button forgeButton;
    public Button dailyQuestButton;
    public Button pvpButton;
    public Button stoneButton;
    public Button dailyDungeonButton;

    private void Start()
    {
        portalButton.onClick.AddListener(_UIManager.PortalPopUp.ShowScreen);
        dailyStoreButton.onClick.AddListener(_UIManager.DailyStorePopUp.ShowScreen);

        //dailyStoreButton.onClick.AddListener(_UIManager.DailyStorePopUp.ShowScreen);


        forgeButton.onClick.AddListener(() => ToastMsg.instance.ShowMessage("개발예정입니다", 0.5f));
        dailyQuestButton.onClick.AddListener(() => ToastMsg.instance.ShowMessage("개발예정입니다", 0.5f));
        pvpButton.onClick.AddListener(() => ToastMsg.instance.ShowMessage("개발예정입니다", 0.5f));
        stoneButton.onClick.AddListener(() => ToastMsg.instance.ShowMessage("개발예정입니다", 0.5f));
        dailyDungeonButton.onClick.AddListener(() => ToastMsg.instance.ShowMessage("개발예정입니다", 0.5f));
    }

}
