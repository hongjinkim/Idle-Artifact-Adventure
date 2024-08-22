using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : PopUp
{
    public HeroPotion heroPotion;


    public void UpdatePotion(string id)
    {
        ShowScreen();
        heroPotion.UpdatePotionInfo(null);
        heroPotion.UpdateHeroPotionItem(id);
    }


}
