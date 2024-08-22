using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
public abstract class Character : MonoBehaviour, IBehavior
{
    //방어력이 1당 damagereduction+=0.2
    public enum UnitState
    {
        idle = 0,
        move = 2,
        attack,
        skill,
        death = 6
    }

    public enum AttackType
    {
        melee,
        Projectile
    }

    public enum CharacterDirection
    {
        down,
        up,
        left,
        Right
    }
    public event Func<Character, float, float> OnTakeDamage;
    public event Action<Character> OnHit;
    
    public event Action<Character> OnKill;
    public event Action<Character, float> OnSkillHit;
    public event Action OnSkillUse;
    public event Action<float> OnUpdate;
    public float SkillDamageMultiplier { get; set; } = 1f;
    public Dungeon dungeon;
    //protected PlayerStat playerStat;
    private List<ISkill> skillList;
    private IObjectPool<DamageText> DamageTextPool;
    public List<GameObject> Parts;
    public List<GameObject> Shadows;

    public Animator animator;

    public GameObject projectilePrefab;

    public GameObject canvas;
    public GameObject PrefabDmgTxt;
    float height = 1f;
    private ConcentrationTrait concentrationTrait;
    private MagicTrait magicTrait;
    private ProtectionTrait protectionTrait;
    public Character _target;

    public Vector2 _tempDistance;
    public Vector2 _dirVec;
    public UnitState _unitState = UnitState.idle;
    public AttackType attackType;
    public CharacterDirection characterDirection;
    public event Action<int> OnMaxHealthChanged;


    public Character attacker;
    private const float MAX_ENERGY = 100f;
    [Header("Infos")]
    public HeroInfo info;
    [SerializeField]
    private int _maxHealth;
    public int maxHealth
    {
        get { return _maxHealth; }
         set
        {
            if (_maxHealth != value)
            {
                _maxHealth = value;
                OnMaxHealthChanged?.Invoke(_maxHealth);
            }
        }
    }
    public int currentHealth;
    public float moveSpeed=1;
    public int attackDamage => info?.attackDamage ?? 1;
    public float attackInterval;
    public int attackRange => info?.attackRange ?? 1;
    public float findRange;
    public int level;
    public float currentExp;
    public float needexp;
    public float findTimer;
    public float attackTimer;

    // 새로 추가된 변수들
    public float Energy
    {
        get { return info != null ? info.energy : 0; }
        set
        {
            if (info != null)
            {
                info.energy = Mathf.Clamp(value, 0, MAX_ENERGY);
            }
        }
    }
    public float attackSpeed=>info?.attackSpeed ?? 100;
    public int hp => info?.hp ?? 100;
    public int defense => info?.defense ?? 10;
    public int magicResistance=>info?.magicResistance ?? 10;
    public float healthRegen=>info?.healthRegen ?? 0;
    public float energyRegen=>info?.energyRegen ?? 5;
    public int expAmplification=>info?.expAmplification??0;
    public int trueDamage=>info?.trueDamage??0;
    public int damageBlock => info?.damageBlock ?? 0;//고정 데미지 감소
    public int lifeSteal => info?.lifeSteal ?? 0;
    public int damageAmplification => info?.damageAmplification ?? 0;
    public int damageReduction => info?.damageReduction ?? 0;
    public int criticalChance => info?.criticalChance ?? 0;
    public int criticalDamage => info?.criticalDamage ?? 150;
    public int defensePenetration => info?.defensePenetration ?? 0;



    [Header("DieEffect")]
    public GameObject fadeObject;
    public SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    public float fadeDuration = 1.0f;

    //[Header("TileMap")]
    // public TilemapManagerGG tilemapManager;
    // public CustomTilemapManagerGG customTilemapManager;
    // public float updatePathInterval = 1f;
    // private int currentPathIndex;
    // private float lastPathUpdateTime = 0f;

    // private Vector3 targetPosition;
    // private Vector3 currentPosition;
    // protected Vector3 nearestValidPosition;
    // private List<Vector3> path;

    // private const float PositionTolerance = 0.13f;
    // private const float PositionToleranceSquared = PositionTolerance * PositionTolerance;

    public Sprite Icon { get; protected set; }
    
    public int Level { get; protected set; }
    public List<Trait> Traits { get; protected set; } = new List<Trait>();
    public TraitType SelectedTraitType { get; protected set; }
    public PlayerSkill ActiveSkill { get; protected set; }

    public float baseAttackInterval = 1f;
    protected HeroSkills skills;

    public bool autoMove;
    public GameObject attackEffectPrefab;
    private float healthRegenTimer = 0f;
    private const float HEALTH_REGEN_INTERVAL = 1f; // 1초마다 HP 회복

    private DungeonManager dungeonManager;

    // 기존 Character 메서드와 Player에서 가져온 메서드 통합
    protected virtual void InitializeSkillBook() { }
    public virtual void IncreaseCharacteristic(float amount) { }

    protected virtual void Awake()
    {
        // Trait 객체들을 초기화합니다.
        concentrationTrait = new ConcentrationTrait();
        magicTrait = new MagicTrait();
        protectionTrait = new ProtectionTrait();
    }
    protected virtual void Start()
    {

        //playerStat = GetComponent<PlayerStat>();
        //if (playerStat == null)
        //{
        //    playerStat = gameObject.AddComponent<PlayerStat>();
        //}
        //customTilemapManager = new CustomTilemapManagerGG(tilemapManager, this);
        //customTilemapManager = gameObject.AddComponent<CustomTilemapManagerGG>();
        //customTilemapManager.Initialize(tilemapManager, this);

        //dungeon._allCharacterList.Add(this);
        //dungeon.DungeonInit();
        //transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        //StartCoroutine(AutoMoveCoroutine());
        if (info != null)
        {
            info.OnHpChanged += UpdateMaxHealth;
            UpdateMaxHealth();
        }
        InitializeHealth();
        dungeonManager = DungeonManager.instance;
        UpdateAttackInterval();
    }
    protected virtual void InitializeHealth()
    {
        currentHealth = maxHealth;
    }
    public void UpdateAttackInterval()
    {

        // attackSpeed가 높을수록 attackInterval이 짧아집니다.
        attackInterval = baseAttackInterval * (100f / attackSpeed);
    }

    protected virtual void Update()
    {
        OnUpdate?.Invoke(Time.deltaTime);
        CheckState();
        ApplyHealthRegen();
        if (currentHealth <= 0 && _unitState != UnitState.death)
        {
            SetState(UnitState.death);
            Die();
        }

    }
    private void ApplyTraits()
    {
        foreach (var appliedTrait in info.appliedTraits)
        {
            Trait trait = GetTrait(appliedTrait.Type);
            if (trait != null)
            {
                trait.ChooseTrait(appliedTrait.Level, appliedTrait.IsLeft);
                trait.ApplyEffect(this);
            }
        }
    }

    private Trait GetTrait(TraitType traitType)
    {
        switch (traitType)
        {
            case TraitType.Concentration:
                return concentrationTrait;
            case TraitType.Magic:
                return magicTrait;
            case TraitType.Protection:
                return protectionTrait;
            default:
                Debug.LogError($"Unknown trait type: {traitType}");
                return null;
        }
    }
    protected IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (info != null)
            {
                info.energy = Mathf.Min(info.energy + info.energyRegen, MAX_ENERGY);
            }
        }
    }
    protected virtual void OnDestroy()
    {
        if (info != null)
        {
            info.OnHpChanged -= UpdateMaxHealth;
        }
    }
    protected virtual void UseSkill()
    {
        OnSkillUse?.Invoke();
        // 기존 스킬 사용 로직...
    }
    protected void CheckAndUseSkill()
    {
        if (Energy >= 100)
        {
            UseSkill();
        }
        
    }
    public void UpdateMaxHealth()
    {
        if (info != null)
        {
            maxHealth = info.hp * 5;
        }
        else
        {
            maxHealth = 100; // 기본값 설정 (필요하다면)
        }

        // 현재 체력이 새로운 최대 체력을 초과하지 않도록 조정
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
    protected virtual void Kill(Character killedCharacter)
    {
        OnKill?.Invoke(killedCharacter);
    }
    // HeroInfo의 hp가 변경될 때 호출되어야 하는 메서드
    public void OnHpChanged()
    {
        UpdateMaxHealth();
    }
    void OnTriggerEnter2D(Collider2D collision) 
    {
        string targetTag = "";

        switch (gameObject.tag)
        {
            case "Hero": 
                targetTag = "Enemy";
                break;

            case "Enemy": 
                targetTag = "Hero";
                break;
        }

        if(collision.gameObject.CompareTag(targetTag))
        {
            
        }
        else if(collision.gameObject.CompareTag(gameObject.tag))
        {
            SetState(UnitState.idle);
        }
    }

    public void Move()
    {
        if (this.gameObject.activeInHierarchy)
        {
            OnMove();
        }
    }

    public virtual void TakeDamage(Character attacker, float damage)
    {
        if (this is Hero && attacker is Hero) return;
        if (OnTakeDamage != null)
        {
            foreach (Func<Character, float, float> handler in OnTakeDamage.GetInvocationList())
            {
                damage = handler(attacker, damage);
            }
        }
        if (this != null)
        {
            this.attacker = attacker;

            // 크리티컬 히트 계산
            bool isCritical = attacker.RollForCritical();
            float criticalMultiplier = isCritical ? attacker.criticalDamage / 100f : 1f;
            float criticalDamage = damage * criticalMultiplier;

            // 데미지 증폭 적용
            float amplifiedDamage = attacker.AmplifyDamage(criticalDamage);

            // 기존 데미지 감소 로직 적용
            float reducedDamage = CalculateReducedDamage(amplifiedDamage);

            // trueDamage 추가
            float totalDamage = reducedDamage + attacker.trueDamage;

            this.currentHealth -= (int)totalDamage;

            // 총 데미지를 하나의 텍스트로 표시 (크리티컬 시 다른 색상으로)
            attacker.InstantiateDmgTxtObj(totalDamage, this.transform.position, isCritical);

            // Life Steal 적용
            attacker.ApplyLifeSteal(totalDamage);
            SoundManager.Instance.PlayHitSound();

        }
        OnHit?.Invoke(this);
    }
    private bool RollForCritical()
    {
        
        return Random.Range(0, 100) < criticalChance;
    }
    private float AmplifyDamage(float originalDamage)
    {
        float amplificationMultiplier = 1 + (damageAmplification / 100f);
        return originalDamage * amplificationMultiplier;
    }
    private void ApplyLifeSteal(float damageDealt)
    {
        if (lifeSteal > 0)
        {
            float healAmount = damageDealt * (lifeSteal / 100f);
            int healedHealth = (int)healAmount;
            currentHealth = Mathf.Min(currentHealth + healedHealth, maxHealth);

            Debug.Log($"{name} healed for {healedHealth} HP from Life Steal. Current health: {currentHealth}");
        }
    }
    private float CalculateReducedDamage(float originalDamage)
    {
        float damageReductionPercentage = damageReduction / 100f; // damageReduction을 백분율로 변환
        float reducedDamage = originalDamage * (1 - damageReductionPercentage);
        return Mathf.Max(reducedDamage, 0); // 데미지가 음수가 되지 않도록 보장
    }
    public virtual void Die()
    {
        // switch (gameObject.tag)
        // {
        //     case "Hero":
        //         GameManager.Instance.dungeonManager._heroUnitList.Remove(this);
        //         break;
        //     case "Enemy":
        //         GameManager.Instance.dungeonManager._enemyUnitList.Remove(this);
        //         break;
        // }

       
        //gameObject.SetActive(false);
        InitFadeEffect();
        StartCoroutine(FadeOut());
        if (attacker != null)
        {
            attacker.OnKill?.Invoke(this);
        }

    }
    public void StartCoroutineFromTrait(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
    private void ApplyHealthRegen()
    {
        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= HEALTH_REGEN_INTERVAL)
        {
            float regenAmount = healthRegen * HEALTH_REGEN_INTERVAL;
            currentHealth = Mathf.Min(currentHealth + (int)regenAmount, maxHealth);
            healthRegenTimer = 0f;
        }
    }

    public bool Alive()
    {
        return currentHealth > 0; 
    }

    public void Attack()
    {
        OnAttck();
    }

    void CheckState()
    {
        switch(_unitState)
        {
            case UnitState.idle:
                FindTarget();
                break;

            case UnitState.move:
                FindTarget();
                Move();
                break;

            case UnitState.attack:
                CheckAttack();
                break;

            case UnitState.skill:
                break;

            case UnitState.death:
                break;
        }
    }

    void SetState(UnitState state)
    {
        _unitState = state;

        switch (_unitState)
        {
            case UnitState.idle:
                SetAnimatorState(_unitState);
                break;

            case UnitState.move:
                SetAnimatorState(_unitState);
                break;

            case UnitState.attack:
                SetAnimatorState(UnitState.idle);
                break;

            case UnitState.skill:
                break;

            case UnitState.death:
                SetAnimatorState(_unitState);
                break;
        }

    }

    void FindTarget()
    {
        // 적을 찾을때 연산이 많아져서 느려질 수 있는 문제를 방지한다.
        findTimer += Time.deltaTime;

        if (dungeon == null)
        {
            return;
        }

        if(findTimer > dungeon._findTimer)
        {
            _target = dungeon.GetTarget(this);

            if (_target != null)
            {
                SetState(UnitState.move);
            }
            else
            {
                SetState(UnitState.idle);
            }

            findTimer = 0;
        }
    }

    bool CheckTarget()
    {
        //if (_target == null) return false;

        bool value = true;

        if(_target == null) value = false;
        if(_target._unitState == UnitState.death) value = false;
        if(!_target.gameObject.activeInHierarchy) value = false;

        if(!value)
        {
            SetState(UnitState.idle);
        }

        return value;
    }

    //void OnMove()
    //{
    //    //if(!CheckTarget()) return;
    //    if(CheckDistance()) return;

    //    _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;

    //    SetDirection();

    //    //transform.position += (Vector3)_dirVec * moveSpeed * Time.deltaTime;
    //    //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    //    MoveAlongPath();

    //    if (path == null || path.Count == 0)
    //    {
    //        SnapToNearestTileCenter();
    //    }
    //}
    void OnMove()
    {
        if(CheckDistance()) return;

        if (_unitState == UnitState.idle)
        {
            return;
        }

        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;

        SetDirection();

        transform.position += (Vector3)_dirVec * moveSpeed * Time.deltaTime;
        

        //MoveAlongPath();

        // if (path == null || path.Count == 0)
        // {
        //     SnapToNearestTileCenter();
        //     UpdatePath(); // 경로가 없으면 새로운 경로를 찾습니다.
        // }
    }

    void SetDirection()
    {
        if (Mathf.Abs(_dirVec.x) > Mathf.Abs(_dirVec.y))
        {
            // 좌우 방향
            if (_dirVec.x >= 0)
            {
                // 오른쪽을 향하도록 설정
                characterDirection = CharacterDirection.Right;
            }
            else
            {
                // 왼쪽을 향하도록 설정
                characterDirection = CharacterDirection.left;
            }
        }
        else
        {
            // 상하 방향
            if (_dirVec.y >= 0)
            {
                // 위를 향하도록 설정
                characterDirection = CharacterDirection.up;
            }
            else
            {
                // 아래를 향하도록 설정
                characterDirection = CharacterDirection.down;
            }
        }

        for (var i = 0; i < Parts.Count; i++)
        {
            Parts[i].SetActive(i == (int)characterDirection);
            Shadows[i].SetActive(i == (int)characterDirection);
        }
    }

    void SetMove()
    {
        
    }

    bool CheckDistance()
    {
        if (_target == null)
        {
            SetState(UnitState.idle);
            return false;
        }

        Vector3 targetPosition = _target.transform.position;
        Vector3 currentPosition = transform.position;

        _tempDistance = (Vector2)(_target.transform.localPosition - transform.position);
        float distanceSquared = _tempDistance.sqrMagnitude;

        if(distanceSquared <= attackRange * attackRange)
        {
            SetState(UnitState.attack);
            return true;
        }
        // if(IsWithinAttackRange(currentPosition, targetPosition, attackRange))
        // {
        //     SetState(UnitState.attack);

        //     return true;
        // }
        else
        {
            if (!CheckTarget())
            {
                SetState(UnitState.idle);
            }
            else
            {
                SetState(UnitState.move);
            }

            return false;
        }
    }

    void CheckAttack()
    {
        if(!CheckTarget()) return;
        if(!CheckDistance()) return;

        attackTimer += Time.deltaTime;

        if(attackTimer > attackInterval)
        {
            Attack();
            attackTimer = 0;
        }
    }

    void OnAttck()
    {
        if (_target == null) return;

        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;
        SetDirection();

        // 애니메이션 삽입
        OnAnimAttack();
        SetAttack();
    }

    void SetAttack()
    {
        if (_target == null) return;

        if (AttackType.Projectile.Equals(attackType))
        {
            if (projectilePrefab != null && _target != null)
            {
                GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Projectile projectile = projectileInstance.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.InitProjectileRotation(_target.transform.position);
                }
                // 원거리 공격의 경우, 투사체가 타겟에 도달했을 때 이펙트 생성
                StartCoroutine(CreateProjectileHitEffect(projectileInstance, _target.transform.position));
            }
        }
        else
        {
            CreateAttackEffect(_target.transform.position);
        }
        _target.TakeDamage(this, attackDamage);
    }
    IEnumerator CreateProjectileHitEffect(GameObject projectile, Vector3 targetPosition)
    {
        // 투사체가 타겟에 도달할 때까지 대기
        while (projectile != null && Vector3.Distance(projectile.transform.position, targetPosition) > 0.1f)
        {
            yield return null;
        }

        // 투사체가 타겟에 도달했거나 파괴되었을 때 이펙트 생성
        CreateAttackEffect(targetPosition);

        // 필요하다면 여기서 투사체를 파괴할 수 있습니다
         if (projectile != null) Destroy(projectile);
    }

    protected virtual void InstantiateDmgTxtObj(float damage, Vector3 targetPosition, bool isCritical)
    {
        GameObject DamageTxtObj = Instantiate(PrefabDmgTxt, dungeonManager.canvasTransform);
        TextMeshProUGUI damageText = DamageTxtObj.GetComponent<TextMeshProUGUI>();
        damageText.text = Mathf.Round(damage).ToString();

        // 크리티컬 히트일 경우 텍스트 색상 변경 및 크기 증가
        if (isCritical)
        {
            damageText.color = Color.red;
            damageText.fontSize *= 1.5f;
        }

        Vector3 damageTxtPos = Camera.main.WorldToScreenPoint(new Vector3(targetPosition.x, targetPosition.y + height, 0));
        DamageTxtObj.GetComponent<RectTransform>().position = damageTxtPos;
    }

    public bool IsAction
    {
        get => animator.GetBool("Action");
        set => animator.SetBool("Action", value);
    }

    public void SetAnimatorState(UnitState state)
	{
        if (animator == null)
        {
            return;
        }

		animator.SetInteger("State", (int) state);

        if (_target == null)
        {
           IsAction = false;    
        }
	}

    public void Slash1H()
	{
		animator.SetTrigger("Slash1H");
        IsAction = true;
    }

    public void ShotBow()
	{
		animator.SetTrigger("ShotBow");
        IsAction = true;
	}

    protected virtual void OnAnimAttack(){Debug.Log("공격애니메이션발동");}

    private void InitFadeEffect()
    {
        spriteRenderers = fadeObject.GetComponentsInChildren<SpriteRenderer>();
        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    // void ManageSkill()
    // {


    //     for (int i = 0; i < length; i++)
    //     {
            
    //     }
    // }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Color newColor = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, alpha);
                spriteRenderers[i].color = newColor;
            }

            yield return null;
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0.0f);
        }

        dungeon._allCharacterList.Remove(this);
        //Destroy(customTilemapManager);
        gameObject.SetActive(false);
    }



    // IEnumerator AutoMoveCoroutine()
    // {
    //     WaitForSeconds wait = new WaitForSeconds(updatePathInterval);

    //     while (true)
    //     {
    //         yield return wait;

    //         UpdatePath();
            
    //     }
    // }

    // void UpdatePath()
    // {
    //     if (_target != null)
    //     {
    //         GameObject closestObject = _target.gameObject;

    //         if (closestObject != null)
    //         {
    //             targetPosition = closestObject.transform.position;
    //             currentPosition = customTilemapManager.GetNearestValidPosition(transform.position);

    //             if ((currentPosition - targetPosition).sqrMagnitude > PositionToleranceSquared)
    //             {
    //                 List<Vector3> surroundingPositions = GetSurroundingPositions(targetPosition);

    //                 Vector3? bestPosition = FindBestPosition(surroundingPositions, currentPosition);

    //                 if (bestPosition.HasValue)
    //                 {
    //                     SetNewPath(bestPosition.Value);
    //                 }
    //                 else
    //                 {
    //                     path = null;
    //                 }
    //             }
    //             else
    //             {
    //                 transform.position = currentPosition;
    //                 path = null;
    //             }
    //         }
            
    //         lastPathUpdateTime = Time.time;
    //     }

    // }

    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>();

        for (int x = -attackRange; x <= 1; x++)
        {
            for (int y = -attackRange; y <= attackRange; y++)
            {
                if (x == 0 && y == 0) continue;

                surroundingPositions.Add(new Vector3(targetPosition.x + x, targetPosition.y + y, targetPosition.z));
            }
        }

        return surroundingPositions;
    }

    // private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    // {
    //     Vector3? bestPosition = null;
    //     float minSqrDistance = float.MaxValue;

    //     for (int i = 0; i < positions.Count; i++)
    //     {
    //         if (customTilemapManager.IsValidMovePosition(positions[i]))
    //         {
    //             float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
    //             if (sqrDistance < minSqrDistance)
    //             {
    //                 minSqrDistance = sqrDistance;
    //                 bestPosition = positions[i];
    //             }
    //         }
    //     }

    //     return bestPosition;
    // }

    // private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    // {
    //     Vector3? bestPosition = null;
    //     float minSqrDistance = float.MaxValue;

    //     for (int i = 0; i < positions.Count; i++)
    //     {
    //         bool isValid = customTilemapManager.IsValidMovePosition(positions[i]);
    //         Debug.Log($"Position {positions[i]} is valid: {isValid}");

    //         if (isValid)
    //         {
    //             float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
    //             Debug.Log($"Distance to {positions[i]}: {sqrDistance}");

    //             if (sqrDistance < minSqrDistance)
    //             {
    //                 minSqrDistance = sqrDistance;
    //                 bestPosition = positions[i];
    //                 Debug.Log($"New best position: {bestPosition}");
    //             }
    //         }
    //     }

    //     Debug.Log($"Final best position: {bestPosition}");
    //     return bestPosition;
    // }

    //protected void SetNewPath(Vector3 target)
    //{
    //    Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
    //    path = customTilemapManager.FindPath(start, target);
    //    currentPathIndex = 0;

    //    if (path != null && path.Count > 0)
    //    {
    //        tilemapManager.SetDebugPath(path);
    //    }
    //}

    // protected void SetNewPath(Vector3 target)
    // {
    //     if (customTilemapManager == null)
    //     {
    //         return;
    //     }

    //     Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
    //     path = customTilemapManager.FindPath(start, target);
    //     currentPathIndex = 0;

    //     if (path != null && path.Count > 0)
    //     {
    //         if (tilemapManager != null)
    //         {
    //             tilemapManager.SetDebugPath(path);
    //         }
    //     }

    // }

    // private void MoveAlongPath()
    // {
    //     if (path != null && currentPathIndex < path.Count)
    //     {
    //         Vector3 targetPosition = path[currentPathIndex];
    //         //targetPosition.z = 0; // 추가: z 값을 0으로 설정

    //         if (!customTilemapManager.IsValidMovePosition(targetPosition))
    //         {
    //             UpdatePath();
    //             return;
    //         }

    //         if ((transform.position - targetPosition).sqrMagnitude > PositionToleranceSquared)
    //         {
    //             Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    //             //newPosition.z = 0; // 추가: z 값을 0으로 설정
    //             transform.position = newPosition;
    //         }
    //         else
    //         {
    //             //transform.position = targetPosition;
    //             //transform.position = new Vector3(targetPosition.x, targetPosition.y, 0); // 추가: z 값을 0으로 설정
    //             currentPathIndex++;
    //         }

    //         if (currentPathIndex >= path.Count)
    //         {
    //             path = null;

    //             SnapToNearestTileCenter();

    //             OnPathComplete();
    //         }
    //     }
    // }

    // protected virtual void OnPathComplete()
    // {
    //     autoMove = true;  // 예시: 플레이어 제어 상태 해제
    //     SetState(UnitState.idle);    // 상태를 idle로 변경
    //     StartCoroutine(WaitAndFindNewPath());
    // }

    // private void SnapToNearestTileCenter()
    // {
    //     nearestValidPosition = customTilemapManager.GetNearestValidPosition(transform.position);
    //     nearestValidPosition.z = 0; // 추가: z 값을 0으로 설정

    //     if ((transform.position - nearestValidPosition).sqrMagnitude < PositionToleranceSquared)
    //     {
    //         transform.position = nearestValidPosition;
    //     }
    // }

    //IEnumerator WaitAndFindNewPath()
    //{
    //    yield return new WaitForSeconds(0.2f);

    //    GameObject closestObject = _target.gameObject;

    //    if (closestObject != null)
    //    {
    //        Vector3 targetPosition = closestObject.transform.position;
    //        SetNewPath(targetPosition);
    //    }

    //}
    // IEnumerator WaitAndFindNewPath()
    // {
    //     yield return new WaitForSeconds(0.2f);

    //     if (_target == null)
    //     {
    //         Debug.LogWarning("Target is null in WaitAndFindNewPath");
    //         SetState(UnitState.idle);
    //         yield break;
    //     }

    //     GameObject closestObject = _target.gameObject;

    //     if (closestObject != null)
    //     {
    //         Vector3 targetPosition = closestObject.transform.position;
    //         SetNewPath(targetPosition);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Closest object is null in WaitAndFindNewPath");
    //         SetState(UnitState.idle);
    //     }
    // }

    // 거리 체크 메서드
    public bool IsWithinAttackRange(Vector3 currentPosition, Vector3 targetPosition, int attackRange)
    {
        // 모든 범위 내의 좌표를 가져옴
        List<Vector3> attackRangePositions = GetAttackableRangePositions(currentPosition, attackRange);

        // targetPosition이 attackRangePositions에 포함되는지 확인
        foreach (var position in attackRangePositions)
        {
            if (position.x == targetPosition.x && position.y == targetPosition.y)
            {
                return true;
            }
        }

        return false;
    }

    private List<Vector3> GetAttackableRangePositions(Vector3 position, int range)
    {
        List<Vector3> attackableRangePositions = new List<Vector3>();

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                // 대각선 포함 모든 타일 좌표 추가
                Vector3 pos = new Vector3(position.x + x, position.y + y, 0);
                attackableRangePositions.Add(pos);
            }
        }

        return attackableRangePositions;
    }
    
    //public abstract void IncreaseCharacteristic(float amount);
    void CreateAttackEffect(Vector3 targetPosition)
    {
        if (attackEffectPrefab != null)
        {
            // 이펙트 생성 위치를 타겟의 위치로 설정
            Vector3 effectPosition = targetPosition;

            // 이펙트 회전 계산 (히어로에서 타겟을 향하는 방향)
            Vector2 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // 이펙트 생성
            GameObject effectInstance = Instantiate(attackEffectPrefab, effectPosition, rotation);

            // 이펙트 크기를 4분의 1로 줄임
            effectInstance.transform.localScale *= 0.25f;

            // 필요하다면 이펙트 지속 시간 후 삭제
            Destroy(effectInstance, 1f); // 1초 후 삭제 (원하는 시간으로 조정 가능)
        }
    }

    public virtual void InstantFadeIn()
    {
        // 모든 스프라이트 렌더러의 알파 값을 1로 설정
        foreach (var renderer in spriteRenderers)
        {
            Color color = renderer.color;
            color.a = 1f;
            renderer.color = color;
        }

        // 모든 파츠를 활성화
        foreach (var part in Parts)
        {
            part.SetActive(true);
        }

        // fadeObject를 활성화
        if (fadeObject != null)
        {
            fadeObject.SetActive(true);
        }
    }
    public virtual void Revive()
    {
        // 상태를 Idle로 변경
        SetState(UnitState.idle);

        foreach (var part in Parts)
        {
            part.SetActive(true);
        }

    }
}
