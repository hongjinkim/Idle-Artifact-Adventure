using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : BaseScreen
{
    private PlayerInfo playerInfo;

    [Header("Texts")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI diamond;
    public TextMeshProUGUI gem;

    [Header("Images")]
    public Image profile;

    private void OnEnable()
    {
        EventManager.StartListening(EventType.FundsUpdated, OnFundsUpdated);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventType.FundsUpdated, OnFundsUpdated);
    }

    void Start()
    {
        playerInfo = GameDataManager.instance.playerInfo;
        EventManager.StartListening(EventType.FundsUpdated, OnFundsUpdated);
        EventManager.StartListening(EventType.BattlePointUpdated, OnBattlePointUpdated);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventType.FundsUpdated, OnFundsUpdated);
        EventManager.StopListening(EventType.BattlePointUpdated, OnBattlePointUpdated);
    }
    void OnFundsUpdated(Dictionary<string, object> message)
    {
        gold.text = NumberFormatter.FormatNumber(playerInfo.gold);
        diamond.text = NumberFormatter.FormatNumber(playerInfo.diamond);
        gem.text = NumberFormatter.FormatNumber(playerInfo.gem);
    }

    void OnLevelUpdated(PlayerInfo info)
    {
        level.text = "Lv. " + info.level.ToString("D2");
    }
    void OnBattlePointUpdated(Dictionary<string, object> message)
    {
        battlePoint.text = NumberFormatter.FormatNumber(GameDataManager.instance.battlePoint);
    }

    public void ShowOnlyFunds()
    {
        //Color color = panel.color;
        //color.a = 0f;
        //panel.color = color;

        profile.gameObject.SetActive(false);
        level.SetActive(false);
        battlePoint.SetActive(false);
    }

    public void ShowPlayerInfo()
    {
        profile.gameObject.SetActive(true);
        level.SetActive(true);
        battlePoint.SetActive(true);
    }
}
