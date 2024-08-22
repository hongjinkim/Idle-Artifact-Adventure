using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static Character;

public class BattleDeckSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Image heroImage;
    private Vector3 originalPosition;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Camera mainCamera;
    [SerializeField] private int slotIndex;

    public Image hpBar;
    private Coroutine hpUpdateCoroutine;

    private int _currentHealth => GameManager.instance.GetCurrentHealth(id);

    [SerializeField] private float updateInterval = 0.1f; // HP 바 업데이트 간격
    private float lastUpdateTime;


    private void Start()
    {
        InitializeComponents();
        mainCamera = Camera.main;
    }

    private void Update()
    {

        if (Time.time - lastUpdateTime >= updateInterval)
        {
            HpBarUpdate();
            lastUpdateTime = Time.time;
        }
    }
    private void InitializeComponents()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if (canvas == null)
            Debug.LogError($"No Canvas found for BattleDeckSlot: {gameObject.name}");
        if (canvasGroup == null)
        {
            Debug.LogWarning($"No CanvasGroup found for BattleDeckSlot: {gameObject.name}. Adding one.");
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetHeroData(HeroInfo hero, int index)
    {
        slotIndex = index;
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;
            speed = hero.agility;
            hp = hero.hp;

            if (heroImage != null)
            {
                // 이미지 로드 및 설정
                Sprite heroSprite = Resources.Load<Sprite>(hero.imagePath);
                if (heroSprite != null)
                {
                    heroImage.sprite = heroSprite;
                    heroImage.enabled = true;
                }
                else
                {
                    Debug.LogWarning($"Failed to load hero image: {hero.imagePath}");
                    heroImage.enabled = false;
                }
            }
            gameObject.SetActive(true);
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        id = 0;
        heroName = "";
        level = 0;
        power = 0;
        speed = 0;
        hp = 0;
        if (heroImage != null)
        {
            heroImage.sprite = null;
            heroImage.enabled = false;
        }
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (id == 0) return;

        originalPosition = rectTransform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (id == 0) return;
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (id == 0) return;

        rectTransform.position = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        HandleDrop(eventData);
    }

    //private void HandleDrop(PointerEventData eventData)
    //{
    //    Vector2 worldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
    //    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

    //    if (hit.collider != null)
    //    {
    //        Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
    //        if (tilemap != null && hit.collider.CompareTag("BattleField"))
    //        {
    //            Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
    //            Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

    //            GameManager.instance.CreateHeroPrefabAtPosition(cellCenter, slotIndex);
    //        }
    //    }
    //}
    private void HandleDrop(PointerEventData eventData)
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("BattleField"))
        {
            Vector3 dropPosition = hit.point;
            GameManager.instance.CreateHeroPrefabAtPosition(dropPosition, slotIndex);
        }
    }

    private void HpBarUpdate()
    {
        hp = HeroManager.instance.Deck[slotIndex].hp;
        int maxHealth = hp * 5;  // 최대 체력은 hp * 5

        if (maxHealth > 0)
        {
            hpBar.fillAmount = (float)_currentHealth / maxHealth;
        }
        else
        {
            hpBar.fillAmount = 0f;
            Debug.LogWarning($"Max health for hero {id} is 0 or less.");
        }
    }
}
