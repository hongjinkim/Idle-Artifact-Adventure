using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class Loot
{
    public string id;
    public float dropRate;
    public int minValue;
    public int maxValue;
    public GameObject droppedItemPrefab;

}
public class Dungeon : MonoBehaviour
{
    private DungeonManager dungeonManager;

    [Header("BasicInformation")]
    //public TilemapManagerGG tilemapManager;
    public string _themeCode;
    public string _themeName;
    public string _stageCode;
    public float _findTimer;
    public int _enemyQuantity;
    public int _bossQuantity = 0;
    private int _randomEnemyIndex;
    private int _randomTileIndex;
    public float bossRespawnTime = 10f;
    private bool isBossRespawning = false;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("ActiveList")]
    public List<Character> _activeEnemyList = new List<Character>();
    public List<Character> _allCharacterList = new List<Character>();
    public List<Character> _activeBossList = new List<Character>();

    [Header("UI_StageSlot")]
    public string _stageName;
    public Sprite _stageImage;
    public int _navigationProgress;

    [Header("UI_StageInformation")]
    public List<Enemy> _enemyPool = new List<Enemy>();
    public List<Enemy> _bossPool = new List<Enemy>();
    public List<Item> _ItemList = new List<Item>();
    public List<Character> _activeHeroList = new List<Character>();

    [Header("Item Loot System")]
    public List<Loot> LootTable = new List<Loot>();
    public Sprite goldSprite;
    [SerializeField] private List<Item> droppedItems = new List<Item>();
    [SerializeField] private List<GameObject> droppedPrefabs = new List<GameObject>();
    [SerializeField] private int droppedGolds = 0;
    [SerializeField] private float totalDropRate = 0;
    public GameObject lootPrefab;

    public int requiredBattlePoint { get; internal set; }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        foreach(Loot loot in LootTable)
        {
            loot.droppedItemPrefab = lootPrefab;
        }

    }
    void Start()
    {   
        //tilemapManager.CalculateTileCenters();
        // 테스트용
        //SetHeroList();

        
        // if (mainCamera != null)
        // {
        //     Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        //     Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
            
        //     spawnAreaMin = new Vector2(bottomLeft.x, bottomLeft.y);
        //     spawnAreaMax = new Vector2(topRight.x, topRight.y);
        // }

        SetEnemyList();
        SetBossList();
        DungeonInit();
        InvokeRepeating("OnPickupItem", 0f, 5f);

        dungeonManager = DungeonManager.instance;
    }

    // 테스트용
    //public List<Character> _heroPool = new List<Character>();
    // void SetHeroList()
    // {
    //    for (var i = 0; i < _heroPool.Count; i++)
    //    {
    //        Character hero = Instantiate(_heroPool[i]);

    //        _activeHeroList.Add(hero);
    //        _allCharacterList.Add(hero);

    //        _activeHeroList[i].gameObject.tag = "Hero";

    //        // 추후에 마우스로 드래그해서 SetActive 하는걸로 변경해야함.
    //        _activeHeroList[i].gameObject.SetActive(true);
    //        //hero.customTilemapManager.allCharacters.Add(hero);
    //    }
    // }

    void Update()
    {
        CheckAllEnemiesDead();
        CheckAllBossesDead();
    }

    public void DungeonInit()
    {
        foreach (var character in _allCharacterList)
        {
            character.dungeon = this;
            //character.tilemapManager = tilemapManager;
        }
    }

    public Character GetTarget(Character unit)
    {
        Character targetUnit = null;
        List<Character> targetList;

        if (unit.CompareTag("Hero"))
        {
            targetList = new List<Character>(_activeEnemyList);
            targetList.AddRange(_activeBossList);
        }
        else
        {
            targetList = _activeHeroList;
        }
        
        float closestDistance = float.MaxValue;

        foreach (var target in targetList)
        {
            if (target == null || !target.gameObject.activeInHierarchy || target._unitState == Character.UnitState.death)
            {
                continue;
            }

            float distanceSquared = ((Vector2)target.transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;

            if (distanceSquared <= unit.findRange * unit.findRange && distanceSquared < closestDistance)
            {
                targetUnit = target;
                closestDistance = distanceSquared;
            }
        }

        return targetUnit;
    }

    void CheckAllEnemiesDead()
    {
        bool allDead = true;

        foreach (var enemy in _activeEnemyList)
        {
            if (enemy != null && !(enemy._unitState == Character.UnitState.death))
            {
                allDead = false;
                break;

            }
        }

        if (allDead)
        {
            List<Character> enemiesToDestroy = new List<Character>();

            foreach (var enemy in _activeEnemyList)
            {
                if (enemy != null)
                {
                    enemiesToDestroy.Add(enemy);
                }
            }

            // 임시로 넣어놓은것이다. 추후에 히어로 추가되고 List에 담기게 해야 된다.
            // Character a = _allCharacterList[0];
            _allCharacterList.Clear();
            // _allCharacterList.Add(a);

            foreach (var hero in _activeHeroList)
            {
                if(hero.gameObject.activeInHierarchy)
                {
                    _allCharacterList.Add(hero);
                }
            }

            foreach (var boss in _activeBossList)
            {
                if(boss.gameObject.activeInHierarchy)
                {
                    _allCharacterList.Add(boss);
                }
            }

            foreach (var enemy in enemiesToDestroy)
            {
                Destroy(enemy.gameObject);
            }

            _activeEnemyList.Clear();
            
            SetEnemyList();
            DungeonInit();
        }
    }

    void CheckAllBossesDead()
    {
        bool allDead = true;

        foreach (var boss in _activeBossList)
        {
            if (boss != null && !(boss._unitState == Character.UnitState.death))
            {
                allDead = false;
                break;
                
            }
        }

        if (allDead && !isBossRespawning)
        {
            isBossRespawning = true;

            List<Character> enemiesToDestroy = new List<Character>();

            foreach (var enemy in _activeBossList)
            {
                if (enemy != null)
                {
                    enemiesToDestroy.Add(enemy);
                }
            }

            // 임시로 넣어놓은것이다. 추후에 히어로 추가되고 List에 담기게 해야 된다.
            // Character a = _allCharacterList[0];
            _allCharacterList.Clear();
            // _allCharacterList.Add(a);

            foreach (var hero in _activeHeroList)
            {
                if(hero.gameObject.activeInHierarchy)
                {
                    _allCharacterList.Add(hero);
                }
            }

            foreach (var enemy in _activeEnemyList)
            {
                if(enemy.gameObject.activeInHierarchy)
                {
                    _allCharacterList.Add(enemy);
                }
            }

            foreach (var enemy in enemiesToDestroy)
            {
                Destroy(enemy.gameObject);
            }

            _activeBossList.Clear();
            
            StartCoroutine(SetBossListCoroutine());
            DungeonInit();
        }
    }

    void SetEnemyList()
    {
        for(var i = 0; i < _enemyQuantity; i++)
        {
            _randomEnemyIndex = Random.Range(0, _enemyPool.Count);
            Enemy enemy = Instantiate(_enemyPool[_randomEnemyIndex]);

            _activeEnemyList.Add(enemy);
            _allCharacterList.Add(enemy);

            _activeEnemyList[i].gameObject.tag = "Enemy";

            // _randomTileIndex = Random.Range(0, tilemapManager.tileCenters.Count);
            // enemy.transform.position = tilemapManager.tileCenters[_randomTileIndex];

            TilemapCollider2D collider = this.transform.GetChild(0).GetChild(0).GetComponent<TilemapCollider2D>();
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            // 추후에 position 변경해야함
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            enemy.transform.position = randomPosition;

            _activeEnemyList[i].gameObject.SetActive(true);
        }
    }

    void SetBossList()
    {
        for(var i = 0; i < _bossQuantity; i++)
        {
            _randomEnemyIndex = Random.Range(0, _bossPool.Count);
            Enemy boss = Instantiate(_bossPool[_randomEnemyIndex]);

            _activeBossList.Add(boss);
            _allCharacterList.Add(boss);

            _activeBossList[i].gameObject.tag = "Enemy";

            // _randomTileIndex = Random.Range(0, tilemapManager.tileCenters.Count);
            // enemy.transform.position = tilemapManager.tileCenters[_randomTileIndex];

            TilemapCollider2D collider = this.transform.GetChild(0).GetChild(0).GetComponent<TilemapCollider2D>();
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            // 추후에 position 변경해야함
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            boss.transform.position = randomPosition;

            _activeBossList[i].gameObject.SetActive(true);
        }

    }

    IEnumerator SetBossListCoroutine()
    {
        yield return new WaitForSeconds(bossRespawnTime);

        for(var i = 0; i < _bossQuantity; i++)
        {
            _randomEnemyIndex = Random.Range(0, _bossPool.Count);
            Enemy boss = Instantiate(_bossPool[_randomEnemyIndex]);

            _activeBossList.Add(boss);
            _allCharacterList.Add(boss);

            _activeBossList[i].gameObject.tag = "Enemy";

            // _randomTileIndex = Random.Range(0, tilemapManager.tileCenters.Count);
            // enemy.transform.position = tilemapManager.tileCenters[_randomTileIndex];

            TilemapCollider2D collider = this.transform.GetChild(0).GetChild(0).GetComponent<TilemapCollider2D>();
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            // 추후에 position 변경해야함
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            boss.transform.position = randomPosition;

            _activeBossList[i].gameObject.SetActive(true);
        }

        isBossRespawning = false;
    }

    // IEnumerator SetBossList()
    // {

    // }

    public void AddHeroToBattlefield(Character hero)
    {
        if (hero != null)
        {
            RemoveHeroFromAllDungeons(hero);

            _activeHeroList.Add(hero);
            _allCharacterList.Add(hero);

            hero.dungeon = this;
            //hero.tilemapManager = tilemapManager;

            // 필요한 경우 추가 초기화
            // hero.Initialize();
        }
    }
    //private void RemoveHeroFromAllDungeons(Character hero)
    //{
    //    // 모든 던전을 순회하며 해당 영웅을 제거
    //    Dungeon[] allDungeons = FindObjectsOfType<Dungeon>();
    //    foreach (Dungeon dungeon in allDungeons)
    //    {
    //        dungeon._activeHeroList.RemoveAll(h => h == hero);
    //        dungeon._allCharacterList.RemoveAll(c => c == hero);
    //    }
    //}

    private void RemoveHeroFromAllDungeons(Character hero) //에러가 나와서 포기?
    {
        // _allDungeonList를 순회하며 해당 영웅을 제거합니다.
        foreach (Dungeon dungeon in dungeonManager._allDungeonList)
        {
            dungeon._activeHeroList.Remove(hero);
            dungeon._allCharacterList.Remove(hero);
        }
    }


    /// Loot System
    public void GetDroppedItem(Transform transform)
    {
        if (totalDropRate == 0)
        {
            foreach (Loot i in LootTable)
            {
                totalDropRate += i.dropRate;
            }
        }
        float randomNumber = Random.value * totalDropRate;
        float cumulativeRate = 0f;
        foreach (Loot i in LootTable)
        {
            cumulativeRate += i.dropRate;
            if (randomNumber <= cumulativeRate)
            {
                if (i.id == "none")
                    return;
                else if (i.id == "gold")
                {
                    droppedGolds += Random.Range(i.minValue, i.maxValue);

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = goldSprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                else
                {
                    var item = new Item(i.id);

                    droppedItems.Add(item);

                    SpriteRenderer renderer = i.droppedItemPrefab.GetComponent<SpriteRenderer>();
                    renderer.sprite = ItemCollection.active.GetItemIcon(item).sprite;
                    // 추후 오브젝트 풇로 변경
                    droppedPrefabs.Add(Instantiate(i.droppedItemPrefab, transform.position, Quaternion.identity));
                }
                return;
            }
        }
        return;
    }

    public void OnPickupItem()
    {
        //골드 획득
        if (droppedGolds > 0)
        {
            GameDataManager.instance.playerInfo.gold += droppedGolds;
            EventManager.TriggerEvent(EventType.ItemPickup, new Dictionary<string, object> { { "gold", (string)(droppedGolds.ToString() + " 골드를 획득 했습니다.") } });
            EventManager.TriggerEvent(EventType.FundsUpdated, null);
        }

        // 아이템 획득
        if (droppedItems != null)
        {
            foreach (Item item in droppedItems)
            {
                if(item.IsEquipment)
                {
                    if(InventoryFull())
                    {
                        EventManager.TriggerEvent(EventType.ItemPickup, new Dictionary<string, object> { { "item", "인벤토리가 가득 찼습니다." } });
                        continue;
                    }
                }

                GameDataManager.instance.AddItem(item);
                EventManager.TriggerEvent(EventType.ItemPickup, new Dictionary<string, object> { { "item", (string)(item.Params.Name) },
                                                                                                    { "rarity",item.Params.Rarity}});
                EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { { "type", item.Params.Type } });
                
            }
            
        }

        // 필드 위애 아이템 표시 모두 제거
        foreach (GameObject go in droppedPrefabs)
        {
            Destroy(go);
        }

        //
        droppedGolds = 0;
        droppedItems.Clear();
        droppedPrefabs.Clear();


        if (ExpScroll.Instance != null)
        {
            ExpScroll.Instance.UpdateExpScrollCount();
        }
    }
    
    private bool InventoryFull()
    {
        int temp = 0;
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Weapon))
            temp += GameDataManager.instance.itemDictionary[ItemType.Weapon].Count;
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Helmet))
            temp += GameDataManager.instance.itemDictionary[ItemType.Helmet].Count;
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Armor))
            temp += GameDataManager.instance.itemDictionary[ItemType.Armor].Count;
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Leggings))
            temp += GameDataManager.instance.itemDictionary[ItemType.Leggings].Count;
        return GameDataManager.instance.playerInfo.itemCapacity <= temp;
    }

    

    


}
