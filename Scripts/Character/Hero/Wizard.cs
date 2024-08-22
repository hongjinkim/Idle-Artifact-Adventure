using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wizard : Hero, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public HeroClass _heroClass;
    private LineRenderer lineRenderer;
    public bool isSelected = false;
    private List<Vector3> previewPath;
    public ScorchedEarth scorchedEarth=new ScorchedEarth();
    public MysticResonance mysticResonance=new MysticResonance();
    public WaveOfHeat waveOfHeat=new WaveOfHeat();

    private float passiveEffectTimer = 0f;
    private const float PASSIVE_EFFECT_INTERVAL = 1f;
    [SerializeField]
    private GameObject scorchedEarthEffectPrefab;
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

        
        info.activeSkill = scorchedEarth;
        Debug.Log($"Wizard initialized with {info.skills.Count} skills. Active skill: {info.activeSkill?.Name ?? "None"}");
        foreach (var skill in info.skills)
        {
            Debug.Log($"Skill: {skill.Name}, Level: {skill.Level}");
        };
    }
    private void SetSkillEffectPrefab()
    {
        if (scorchedEarthEffectPrefab != null)
        {
            scorchedEarth.SetEffectPrefab(scorchedEarthEffectPrefab);
        }
        else
        {
            Debug.LogWarning("Scorched Earth effect prefab is not assigned in Wizard!");
        }
    }
    public void SkillInit()
    {
        info.SetCharacter(this);
        info.skills.Add(scorchedEarth);
        info.skills.Add(mysticResonance);
        info.skills.Add(waveOfHeat);

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
        ApplyWaveOfHeat();
    }

    private void ApplyWaveOfHeat()
    {
        int attackSpeedReduction = waveOfHeat.GetAttackSpeedReduction();
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, info.attackRange);

        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplyAttackSpeedReduction(attackSpeedReduction);
                }
            }
        }
    }
    protected override void UseSkill()
    {
        Debug.Log($"Wizard UseSkill method called. Current Energy: {Energy}");

        if (Energy >= 100 && info.activeSkill != null)
        {
            Debug.Log($"Wizard energy is full, using {info.activeSkill.Name}");

            // MysticResonance 효과 적용
            if (info.activeSkill is ScorchedEarth scorchedEarth)
            {
                float damageMultiplier = 1f + mysticResonance.GetSkillDamageAmplification();
                scorchedEarth.SetDamageMultiplier(damageMultiplier);
            }

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
