using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MainScreen
{
    public InventoryBase inventoryBase;

    [Header("Buttons")]
    public Button EquipMenuButton;
    public Button UsableMenuButton;
    public Button MaterialMenuButton;
    public Button CrystalMenuButton;
    public Button DisassemblyMenuButton;

    private void Start()
    {
        EquipMenuButton.onClick.AddListener(inventoryBase.OnEquipmentButtonClicked);
        UsableMenuButton.onClick.AddListener(inventoryBase.OnUsableButtonClicked);
        MaterialMenuButton.onClick.AddListener(inventoryBase.OnMaterialButtonClicked);
        CrystalMenuButton.onClick.AddListener(inventoryBase.OnCrystalButtonClicked);
        DisassemblyMenuButton.onClick.AddListener(inventoryBase.OnDisassemblyButtonClicked);
    }




}
