using UnityEngine;
using UnityEngine.UI;

public class DeckSlot : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Button unequipButton;
    private HeroManager heroManager;
    private int deckIndex;

    private void Awake()
    {
        heroManager = FindObjectOfType<HeroManager>();
        unequipButton.onClick.AddListener(OnUnequipButtonClick);
    }

    private void OnUnequipButtonClick()
    {
        heroManager.RemoveHeroFromDeck(deckIndex);
    }

    public void DeckSetHeroData(HeroInfo hero, int index)
    {
        deckIndex = index;
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;
            speed = hero.agility;
            hp = hero.hp;

            // 이미지 로드 및 설정
            if (heroImage != null)
            {
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
        gameObject.SetActive(true);  // 비어있는 슬롯도 보이도록 함
    }

    public bool CheckUIElements()
    {
        return heroImage != null;
    }
}