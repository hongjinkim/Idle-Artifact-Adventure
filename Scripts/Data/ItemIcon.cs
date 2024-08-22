using System;
using UnityEngine;


[Serializable]
public class ItemIcon
{
    public string name;
    public string id;
    public string edition;
    public string collection;
    public string path;
    public Sprite sprite;

    public ItemIcon(string edition, string collection, string type, string name, string path, Sprite sprite)
    {
        this.id = $"{edition}.{collection}.{type}.{name}";
        this.name = name;
        this.collection = collection;
        this.edition = edition;
        this.path = path;
        this.sprite = sprite;
    }
}