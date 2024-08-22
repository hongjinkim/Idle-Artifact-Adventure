using System.Collections.Generic;
using UnityEngine;

public class RandomStat : MonoBehaviour
{
    private int maxTotalStats = 200;
    private int curTotalStats;
    public List<string> statName = new List<string>();
    public List<int> stats = new List<int>();

    private void Start()
    {
        CreateItemStats();
    }

    private void CreateItemStats()
    {
        int randomStatCount = Random.Range(1, 5);
        for (int i = 0; i < randomStatCount; i++)
        {
            statName.Add("Stat" + i);
            stats.Add(0);
        }
        for (int i = 0; i < stats.Count; i++)
        {
            int randBal = Random.Range(0, 11); // ¸Æ½ºÄ¡ º¸Á¤
            int rand = Random.Range(0, maxTotalStats - randBal);
            stats[i] = rand;
            curTotalStats += stats[i];
            maxTotalStats -= stats[i];
        }
    }
}
