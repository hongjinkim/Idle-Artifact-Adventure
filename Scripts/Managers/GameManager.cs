using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private DungeonManager dungeonManager;
    private HeroManager heroManager;
    private int maxDeckSize = 4;
    public List<Character>heroCharacterScript=new List<Character>();
    [SerializeField] private GameObject KnightPrefab;
    [SerializeField] private GameObject WizardPrefab;
    [SerializeField] private GameObject PriestPrefab;
    [SerializeField] private GameObject ArcherPrefab;
    [SerializeField] private Dictionary<int, GameObject> heroPrefabs = new Dictionary<int, GameObject>();
    public List<GameObject> HeroDeckPrefab = new List<GameObject>();

    public Transform pickUpGrid;

    private void Start()
    {
        HeroDeckPrefab = new List<GameObject>(new GameObject[maxDeckSize]);

        heroManager = HeroManager.instance;
        Invoke("InitializeHeroPrefabs", 0.1f);

        // 초기 덱 상태 반영
        for (int i = 0; i < heroManager.Deck.Count; i++)
        {
            UpdateHeroDeckPrefab(i, heroManager.Deck[i].id);
        }
    }

    private void InitializeHeroPrefabs()
    {
        heroPrefabs.Clear();
        if (heroManager != null && heroManager.AllHeroes != null)
        {
            GameObject[] prefabArray = { KnightPrefab, WizardPrefab, PriestPrefab, ArcherPrefab };
            for (int i = 0; i < heroManager.AllHeroes.Count && i < prefabArray.Length; i++)
            {
                GameObject prefab = prefabArray[i];
                if (prefab != null)
                {
                    int heroId = heroManager.AllHeroes[i].id; // 1번 프리팹의 id = 1001
                    GameObject instance = Instantiate(prefab);

                    instance.GetComponent<Character>().info = GameDataManager.instance.playerInfo.heroes[i];
                    heroCharacterScript.Add(instance.GetComponent<Character>());
                    instance.SetActive(false);
                    heroPrefabs[heroId] = instance;

                }
            }
        }
    }


    public void CreateHeroPrefabAtPosition(Vector3 position, int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < HeroDeckPrefab.Count && HeroDeckPrefab[slotIndex] != null)
        {
            int heroId = heroManager.GetHeroIdFromDeckSlot(slotIndex);
            if (heroPrefabs.TryGetValue(heroId, out GameObject heroInstance))
            {
                heroInstance.transform.position = position;
                heroInstance.transform.rotation = Quaternion.identity;
                heroInstance.SetActive(true);

                Character hero = heroInstance.GetComponent<Character>();
                if (hero != null)
                {
                    hero.gameObject.tag = "Hero";

                    Dungeon dd = DungeonManager.instance._selectDungeon;
                    if (dd != null)
                    {
                        dd.AddHeroToBattlefield(hero);
                    }
                }
            }
        }
    }

    public GameObject GetHeroPrefab(int id)
    {
        if (heroPrefabs.TryGetValue(id, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    public List<int> GetAllHeroIds()
    {
        return new List<int>(heroPrefabs.Keys);
    }

    ///

    public void UpdateHeroDeckPrefab(int index, int heroId)
    {
        if (heroPrefabs.TryGetValue(heroId, out GameObject prefab))
        {
            HeroDeckPrefab[index] = prefab;
        }
    }

    public int GetCurrentHealth(int heroId)
    {
        if(heroPrefabs.TryGetValue(heroId, out GameObject heroInstance))
        {
            return heroInstance.GetComponent<Character>().currentHealth;
        }
        return 0;
    }

}