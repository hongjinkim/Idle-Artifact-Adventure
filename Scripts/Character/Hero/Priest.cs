using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Priest : Hero, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public HeroClass _heroClass;
    private LineRenderer lineRenderer;
    public bool isSelected = false;
    private List<Vector3> previewPath;
    public PurifyingLight purifyingLight = new PurifyingLight();
    public HolyGrace holyGrace = new HolyGrace();
    public DazzlingLight dazzlingLight = new DazzlingLight();
    public MysticalPower mysticalPower = new MysticalPower();
    private float passiveEffectTimer = 0f;
    private const float PASSIVE_EFFECT_INTERVAL = 1f;
    private Dictionary<Hero, bool> affectedHeroes = new Dictionary<Hero, bool>();
    [SerializeField]
    private GameObject purifyingLightEffectPrefab;
    private void Awake() 
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    protected override void Start()
    {
        base.Start();

        SkillInit();
        SetSkillEffectPrefab();
        _heroClass = HeroClass.Priest;
        info.characteristicType = CharacteristicType.Intellect;
        info.attackRange = 4;

        
        info.activeSkill = purifyingLight;
        Debug.Log($"Priest initialized with {info.skills.Count} skills. Active skill: {info.activeSkill?.Name ?? "None"}");
        foreach (var skill in info.skills)
        {
            Debug.Log($"Skill: {skill.Name}, Level: {skill.Level}");
        }
    }
    private void SetSkillEffectPrefab()
    {
        if (purifyingLightEffectPrefab != null)
        {
            purifyingLight.SetEffectPrefab(purifyingLightEffectPrefab);
        }
        else
        {
            Debug.LogWarning("Purifying Light effect prefab is not assigned in Priest!");
        }
    }
    public void SkillInit()
    {
        
        info.skills.Add(purifyingLight);
        info.skills.Add(holyGrace);
        info.skills.Add(dazzlingLight);
        info.skills.Add(mysticalPower);    

    }
    protected override void Update()
    {
        base.Update();
        CheckAndUseSkill();
        passiveEffectTimer += Time.deltaTime;
        if (passiveEffectTimer >= PASSIVE_EFFECT_INTERVAL)
        {
            ApplyPassiveEffects();
            passiveEffectTimer = 0f;
        }
    }
    private void ApplyPassiveEffects()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, info.attackRange);

        HashSet<Hero> currentHeroes = new HashSet<Hero>();

        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.CompareTag("Hero"))
            {
                Hero hero = collider.GetComponent<Hero>();
                if (hero != null && hero != this)
                {
                    currentHeroes.Add(hero);
                    if (!affectedHeroes.ContainsKey(hero) || !affectedHeroes[hero])
                    {
                        ApplyHolyGrace(hero);
                        affectedHeroes[hero] = true;
                    }
                }
            }
            else if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    ApplyDazzlingLight(enemy);
                }
            }
        }
        List<Hero> heroesToRemove = new List<Hero>();
        foreach (var pair in affectedHeroes)
        {
            if (!currentHeroes.Contains(pair.Key))
            {
                RemoveHolyGrace(pair.Key);
                heroesToRemove.Add(pair.Key);
            }
        }

        foreach (var hero in heroesToRemove)
        {
            affectedHeroes.Remove(hero);
        }
    }
    private void ApplyHolyGrace(Hero hero)
    {
        holyGrace.ApplyEffect(hero);
    }

    private void RemoveHolyGrace(Hero hero)
    {
        holyGrace.RemoveEffect(hero);
    }
    public void RemoveAllPassiveEffects()
    {
        holyGrace.RemoveAllEffects();
        // 다른 패시브 스킬들에 대해서도 같은 작업 수행
        affectedHeroes.Clear();
    }

    private void ApplyDazzlingLight(Enemy enemy)
    {
        enemy.info.defense -= dazzlingLight.GetDefenseReduction();
        enemy.info.magicResistance -= dazzlingLight.GetMagicResistanceReduction();

        // 방어력과 마법 저항력이 음수가 되지 않도록 보장
        enemy.info.defense = Mathf.Max(0, enemy.info.defense);
        enemy.info.magicResistance = Mathf.Max(0, enemy.info.magicResistance);
    }

    private void ApplyMysticalPower(Hero hero)
    {
        hero.Energy += mysticalPower.GetEnergyRegeneration();
    }
    protected override void UseSkill()
    {
        Debug.Log($"Priest UseSkill method called. Current Energy: {Energy}");

        if (Energy >= 100 && info.activeSkill != null)
        {
            Debug.Log($"Priest energy is full, using {info.activeSkill.Name}");
            info.activeSkill.UseSkill(this);
            Energy = 0;  // 스킬 사용 후 에너지 리셋
        }
    }
    protected override void OnAnimAttack()
    {
        animator.SetTrigger("Slash1H");
        IsAction = true;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        lineRenderer.positionCount = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        //Vector3 nearestValidEndPosition = base.customTilemapManager.GetNearestValidPosition(endPosition);

        // if (customTilemapManager.IsValidMovePosition(nearestValidEndPosition))
        // {
        //     Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        //     previewPath = customTilemapManager.FindPath(start, nearestValidEndPosition);
        //     DrawPath(previewPath);
        // }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleSelectionAndMovement();
        isSelected = false;

        lineRenderer.positionCount = 0;
    }

    private void DrawPath(List<Vector3> pathToDraw)
    {
        if (pathToDraw != null && pathToDraw.Count > 0)
        {
            lineRenderer.positionCount = pathToDraw.Count;
            lineRenderer.SetPositions(pathToDraw.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public void HandleSelectionAndMovement()
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        // base.nearestValidPosition = customTilemapManager.GetNearestValidPosition(endPosition);

        // if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
        // {
        //     base.SetNewPath(nearestValidPosition);
        // }
    }
}
