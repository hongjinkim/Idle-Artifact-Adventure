using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Prize
{
    public string id;
    public string prizeName;
    public int price;
    public Item needmaterial;
    
    public GameObject prize;
}
