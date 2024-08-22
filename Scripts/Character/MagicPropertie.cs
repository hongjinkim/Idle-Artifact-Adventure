using System.Collections.Generic;
using UnityEngine;

public class MagicPropertie : MonoBehaviour
{
    public List<string> magicPropertyNames = new List<string>();
    public List<int> magicPropertyStats = new List<int>();
    public int minStat = 30;
    public int maxStat = 15;
    public string type;
    public int[] magicArray = new int[4];

    public void Start()
    {
        type = "def";
        MagicCreate();
        MagicValue();
        ImgLoader();
    }

    public void MagicCreate()
    {
        switch (type)
        {
            case "pow":
                magicArray = new int[] { 1 };
                break;
            case "sed":
                magicArray = new int[] { 1, 2 };
                break;
            case "def":
                magicArray = new int[] { 1, 2, 3 };
                break;
            case "atk":
                magicArray = new int[] { 1, 2, 3, 4 };
                break;
            default:
                break;
        }


        magicPropertyStats.Clear(); 
        for (int i = 0; i < magicArray.Length; i++)
        {
            magicPropertyStats.Add(0);
        }
    }

    private void MagicValue()
    {
        for (int i = 0; i < magicArray.Length; i++)
        {
            int rand = Random.Range(minStat, maxStat);
            magicPropertyStats[i] = rand;
        }
    }

    private void ImgLoader()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("ItemImg/1");
    }
}
