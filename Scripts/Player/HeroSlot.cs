using UnityEngine;
using UnityEngine.UI;

public class HeroSlot : MonoBehaviour
{
    [SerializeField] private Button equipButton;
    [SerializeField] private Image heroImage; // 새로 추가된 Image 컴포넌트
    [SerializeField] private int heroId;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int attackDamage;
    [SerializeField] private int agility;
    [SerializeField] private int hp;
    private HeroManager heroManager;
    private int myHeroIndex;
    private HeroInfo currentHero;

    private void Awake()
    {
        heroManager = FindObjectOfType<HeroManager>();
        equipButton.onClick.AddListener(OnEquipButtonClick);
    }

    private void OnEquipButtonClick()
    {
        heroManager.AddHeroToDeck(myHeroIndex);
    }

    public void SetHeroData(HeroInfo hero, int index)
    {
        currentHero = hero;
        myHeroIndex = index;
        if (hero != null)
        {
            heroId = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            attackDamage = hero.attackDamage;
            agility = hero.agility;
            hp = hero.hp;

            // 이미지 로드 및 설정
            Sprite heroSprite = Resources.Load<Sprite>(hero.imagePath);
            if (heroSprite != null)
            {
                heroImage.sprite = heroSprite;
                heroImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Failed to load hero image: {hero.imagePath}");
                heroImage.gameObject.SetActive(false);
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
        currentHero = null;
        myHeroIndex = -1;
        heroId = 0;
        heroName = "";
        level = 0;
        attackDamage = 0;
        agility = 0;
        hp = 0;
        heroImage.sprite = null;
        heroImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public HeroInfo GetHeroInfo()
    {
        return currentHero;
    }
}