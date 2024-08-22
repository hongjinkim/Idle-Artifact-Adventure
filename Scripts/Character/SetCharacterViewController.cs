using UnityEngine;
using UnityEngine.UI;

public class SetCharacterViewController : MonoBehaviour
{
    private Character character;

    public GameObject canvas;

    public GameObject prefabHpBar;
    GameObject hpBarObj;
    RectTransform hpBar;
    Image currentHpBarAmount;

    public GameObject prefabEnergyBar;
    GameObject energyBarObj;
    RectTransform energyBar;
    Image currentEnergyBarAmount;

    float height = 1f;

    private Camera mainCamera;

    private DungeonManager dungeonManager;
   

    private void Awake() 
    {
        character = GetComponent<Character>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        dungeonManager = DungeonManager.instance;
        InitHpBar();
        if (character.info != null)
        {
            InitEnergyBar();
        }
    }
    public void InitEnergyBar()
    {
        if (prefabEnergyBar != null)
        {
            energyBarObj = Instantiate(prefabEnergyBar, dungeonManager.canvasTransform);

            energyBar = energyBarObj.GetComponent<RectTransform>();
            currentEnergyBarAmount = energyBar.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        }
    }

    private void Update()
    {
        CalPosHpBar();
        ControlViewHpBar();
        if (character.info != null)
        {
            CalPosEnergyBar(); // 새로 추가
            ControlViewEnergyBar();
        }// 새로 추가
        ControlActive();
    }
    public void CalPosEnergyBar()
    {
        if (energyBar != null)
        {
            Vector3 _energyBarPos = mainCamera.WorldToScreenPoint(new Vector3(character.transform.position.x, character.transform.position.y + height + 0.2f, 0));
            energyBar.position = _energyBarPos;
        }
    }

    public void ControlViewEnergyBar()
    {
        if (character is Hero hero)
        {
            currentEnergyBarAmount.fillAmount = hero.info.energy / 100f; // 에너지의 최대값이 100이라고 가정
        }
    }
    public void InitHpBar()
    {
        hpBarObj = Instantiate(prefabHpBar , dungeonManager.canvasTransform);
        hpBar = hpBarObj.GetComponent<RectTransform>();
        currentHpBarAmount = hpBar.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void CalPosHpBar()
    {
        Vector3 _hpBarPos = mainCamera.WorldToScreenPoint(new Vector3(character.transform.position.x, character.transform.position.y + height, 0));
        hpBar.position = _hpBarPos;
    }

    public void ControlViewHpBar()
    {
        currentHpBarAmount.fillAmount = (float)character.currentHealth / (float)character.maxHealth;
    }

    public void ControlActive()
    {
        if (currentHpBarAmount.fillAmount <= 0)
        {
            hpBarObj.SetActive(false);

            if (energyBar != null)
            {
                energyBarObj.SetActive(false);
            }
        }
        else if (currentHpBarAmount.fillAmount > 0 && !hpBarObj.activeSelf)
        {
            hpBarObj.SetActive(true);


            if (energyBar != null)
            {
                energyBarObj.SetActive(true);
            }
        }
    }

}
