using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager>
{
    public Transform canvasTransform;

    public List<Dungeon> _allDungeonPrefabList = new List<Dungeon>();
    public List<Dungeon> _allDungeonList = new List<Dungeon>();
    public List<Dungeon> _themeList = new List<Dungeon>();

    //public PopupManager popupManager;
    public Dungeon _selectDungeon;
    public DungeonTheme dungeonTheme;

    private MainUIManager _UIManager;

    private int requiredBattlePoint;
    public int required = 0;

    protected override void Awake()
    {
        base.Awake();
        _UIManager = MainUIManager.instance;
    }

    private void Start()
    {
        ArrangeDungeon();
        _selectDungeon = _allDungeonList[0];
    }

    public void ArrangeDungeon()
    {
        float xOffset = 6.0f;
        float spacing = 6.0f;

        foreach (var dungeon in _allDungeonPrefabList)
        {
            Dungeon dungeonInstance = Instantiate(dungeon, new Vector3(xOffset, 0, 0), Quaternion.identity);
            
            dungeonInstance.spawnAreaMin.x = xOffset;
            xOffset += spacing;
            dungeonInstance.spawnAreaMax.x = xOffset;

            dungeonInstance.requiredBattlePoint = 1500 + (int)(required * 50); // 입장 제한
            required += 10;

            _allDungeonList.Add(dungeonInstance);
        }
        
    }

    //public void OpenAdventurePopup(string _themeCode)
    //{
    //    _themeList.Clear();

    //    foreach (var dungeon in _allDungeonList)
    //    {
    //        if (dungeon._themeCode.Equals(_themeCode) && !_themeList.Contains(dungeon))
    //        {
    //            _themeList.Add(dungeon);
    //        }
    //    }

    //    //popupManager.InitAdventurePopup(_themeList);
    //    if (GameDataManager.instance.battlePoint <= _selectDungeon.requiredBattlePoint)
    //    {
    //        ToastMsg.instance.ShowMessage("배틀포인트가 부족합니다\n" + (_selectDungeon.requiredBattlePoint - GameDataManager.instance.battlePoint) + "만큼 부족", 2.0f);
    //        return;
    //    }
    //    else
    //    {
    //        SelectDungeon(0);
    //        dungeonTheme.InitAdventurePopup(_themeList);
    //        //popupManager.adventurePopup.SetActive(true);
    //    }

    //}

    public void SelectDungeon(int index)
    {
        if (GameDataManager.instance.battlePoint <= _themeList[index].requiredBattlePoint)
        {
            ToastMsg.instance.ShowMessage("배틀포인트가 부족합니다\n" + (_themeList[index].requiredBattlePoint - GameDataManager.instance.battlePoint) + "부족", 1.5f);
            return;
        }
        else
        {
            _selectDungeon = _themeList[index];
            dungeonTheme.themeNameText.text = _themeList[index]._stageName.ToString();
        }

    }

    public void EnterDungeon()
    {
        ChangeCameraPos(_selectDungeon.transform.position);
        _UIManager.ShowBattleScreen();
        EventManager.TriggerEvent(EventType.DungeonEntered, null);
    }

    public void ChangeCameraPos(Vector3 position)
    {
        Camera.main.transform.position = new Vector3(position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    // void SetUnitList()
    // {
    //     _ActiveHeroList.Clear();
    //     _ActiveEnemyList.Clear();

    //     //_heroUnitList[0].

    //     for(var i = 0; i < _unitPool.Count; i++)
    //     {
    //         for(var j = 0; j < _unitPool[i].childCount; j++)
    //         {
    //             switch(i)
    //             {
    //                 case 0:
    //                     _ActiveHeroList.Add(_unitPool[i].GetChild(j).GetComponent<Character>());
    //                     _unitPool[i].GetChild(j).gameObject.tag = "Hero";
    //                     break;

    //                 case 1:
    //                     _ActiveEnemyList.Add(_unitPool[i].GetChild(j).GetComponent<Character>());
    //                     _unitPool[i].GetChild(j).gameObject.tag = "Enemy";
    //                     break;
    //             }
    //         }
    //     }
    // }

    // void SetHeroList()
    // {
    //     for (var i = 0; i < _heroPool.Count; i++)
    //     {
    //         Character hero = Instantiate(_heroPool[i]);

    //         _activeHeroList.Add(hero);
    //         _allCharacterList.Add(hero);

    //         _activeHeroList[i].gameObject.tag = "Hero";

    //         // 추후에 마우스로 드래그해서 SetActive 하는걸로 변경해야함.
    //         _activeHeroList[i].gameObject.SetActive(true);
    //         //hero.customTilemapManager.allCharacters.Add(hero);
    //     }
    // }

    // void SetEnemyList()
    // {
    //     for(var i = 0; i < enemyQuantity; i++)
    //     {
    //         randomEnemyIndex = Random.Range(0, _enemyPool.Count);
    //         Character enemy = Instantiate(_enemyPool[randomEnemyIndex]);

    //         _activeEnemyList.Add(enemy);
    //         _allCharacterList.Add(enemy);

    //         _activeEnemyList[i].gameObject.tag = "Enemy";

    //         randomeTileIndex = Random.Range(0, TilemapManagerGG.Instance.tileCenters.Count);
    //         enemy.transform.position = TilemapManagerGG.Instance.tileCenters[randomeTileIndex];

    //         // 랜덤한 위치 생성
    //         // Vector2 randomPosition = new Vector2(
    //         //     Random.Range(spawnAreaMin.x, spawnAreaMax.x),
    //         //     Random.Range(spawnAreaMin.y, spawnAreaMax.y)
    //         // );
    //         //BoundsInt bounds = enemy.customTilemapManager.tilemap.cellBounds;

    //         //Vector2 randomPosition = GetRandomTilemapPosition(_ActiveEnemyList[i]);

    //         //enemy.transform.position = randomPosition;
    //         _activeEnemyList[i].gameObject.SetActive(true);
    //         //enemy.customTilemapManager.allCharacters.Add(enemy);

    //     }
    // }

    // Vector2 GetRandomTilemapPosition(Character enemy)
    // {
    //     // 타일맵 영역 범위
    //     BoundsInt bounds = enemy.customTilemapManager.tilemap.cellBounds;

    //     // 랜덤한 위치 생성
    //     Vector2 randomPosition = new Vector2(
    //         Random.Range(bounds.min.x, bounds.max.x),
    //         Random.Range(bounds.min.y, bounds.max.y)
    //     );

    //     // 랜덤 위치를 타일맵 기준으로 조정하여 유효한 위치로 반환
    //     return enemy.customTilemapManager.GetNearestValidPosition(randomPosition);
    // }

    // void CheckAllEnemiesDead()
    // {
    //     // foreach (var enemy in _ActiveEnemyList)
    //     // {
    //     //     if (enemy != null && !(enemy._unitState == Character.UnitState.death))
    //     //     {
    //     //         return;
    //     //     }
    //     // }

    //     // foreach (var enemy in _ActiveEnemyList)
    //     // {
    //     //     Destroy(enemy);
    //     // }

    //     // _ActiveEnemyList.Clear();
    //     // SetEnemyList();

    //     bool allDead = true;

    //     foreach (var enemy in _activeEnemyList)
    //     {
    //         if (enemy != null && !(enemy._unitState == Character.UnitState.death))
    //         {
    //             allDead = false;
    //             break;
    //         }
    //     }

    //     if (allDead)
    //     {
    //         List<Character> enemiesToDestroy = new List<Character>();

    //         foreach (var enemy in _activeEnemyList)
    //         {
    //             if (enemy != null)
    //             {
    //                 enemiesToDestroy.Add(enemy);
    //             }
    //         }

    //         // foreach (var enemy in _ActiveEnemyList)
    //         // {
    //         //     enemy.customTilemapManager.allCharacters.Clear();
    //         // }

    //         // foreach (var hero in _ActiveHeroList)
    //         // {
    //         //     hero.customTilemapManager.allCharacters.Clear();
    //         // }

    //         _allCharacterList.Clear();

    //         foreach (var enemy in enemiesToDestroy)
    //         {
    //             //_allCharacterList.Remove(enemy);
    //             Destroy(enemy.gameObject);
    //         }

    //         _activeEnemyList.Clear();

    //         SetEnemyList();
    //     }
    // }

    // public Character GetTarget(Character unit)
    // {
    //     Character targetUnit = null;

    //     List<Character> targetList = new List<Character>();

    //     switch (unit.tag)
    //     {
    //         case "Hero":
    //             targetList = _activeEnemyList;
    //             break;

    //         case "Enemy":
    //             targetList = _activeHeroList;
    //             break;
    //     }

    //     // closestDistance: 가장 가까운 유닛과의 거리를 저장하는 변수입니다. 초기값을 매우 큰 값(float.MaxValue)으로 설정하여 첫 번째 비교에서 무조건 갱신되도록 합니다.
    //     float closestDistance = float.MaxValue;

    //     for (var i = 0; i < targetList.Count; i++)
    //     {
    //         // distanceSquared: 현재 유닛과 unit 간의 거리의 제곱입니다. Vector2를 사용하여 계산하며, sqrMagnitude를 사용하여 제곱 값을 얻습니다. 거리 제곱을 사용하는 이유는 루트 계산을 생략하여 성능을 최적화하기 위함입니다.
    //         float distanceSquared = ((Vector2)targetList[i].transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;

    //         // unit.findRange * unit.findRange: 적을 찾는 범위의 제곱입니다. 거리를 비교할 때 제곱 값을 사용하여 루트 계산을 피합니다.
    //         if(distanceSquared <= unit.findRange * unit.findRange)
    //         {
    //             if(targetList[i].gameObject.activeInHierarchy)
    //             {
    //                 if(targetList[i]._unitState != Character.UnitState.death)
    //                 {
    //                     if(distanceSquared < closestDistance)
    //                     {
    //                         targetUnit = targetList[i];
    //                         closestDistance = distanceSquared;
    //                     }
    //                 }
    //             }
    //         }
    //     }

    //     return targetUnit;
    // }

    // public Character GetTarget(Character unit)
    // {
    //     Character targetUnit = null;
    //     List<Character> targetList = unit.CompareTag("Hero") ? _activeEnemyList : _activeHeroList;
    //     float closestDistance = float.MaxValue;

    //     foreach (var target in targetList)
    //     {
    //         if (target == null || !target.gameObject.activeInHierarchy || target._unitState == Character.UnitState.death)
    //         {
    //             continue;
    //         }

    //         float distanceSquared = ((Vector2)target.transform.localPosition - (Vector2)unit.transform.localPosition).sqrMagnitude;

    //         if (distanceSquared <= unit.findRange * unit.findRange && distanceSquared < closestDistance)
    //         {
    //             targetUnit = target;
    //             closestDistance = distanceSquared;
    //         }
    //     }

    //     return targetUnit;
    // }

}
