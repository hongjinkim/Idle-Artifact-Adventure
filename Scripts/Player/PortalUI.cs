using UnityEngine;

public class PortalUI : MonoBehaviour
{
    public GameObject heroSlotPrefab;
    public Transform activeHeroesContainer;
    public Transform allHeroesContainer;

    private HeroManager heroManager;

    private void Start()
    {
        heroManager = FindObjectOfType<HeroManager>();
        //UpdateUI();
    }

    private void UpdateUI()
    {
        //ClearContainers();
        //PopulateActiveHeroes();
        //PopulateAllHeroes();
    }

    //private void ClearContainers()
    //{
    //    foreach (Transform child in activeHeroesContainer) Destroy(child.gameObject);
    //    foreach (Transform child in allHeroesContainer) Destroy(child.gameObject);
    //}

    //private void PopulateActiveHeroes()
    //{
    //    foreach (var hero in heroManager.ActiveHeroes)
    //    {
    //        CreateHeroSlot(hero, activeHeroesContainer, true);
    //    }
    //}

    //private void PopulateAllHeroes()
    //{
    //    foreach (var hero in heroManager.AllHeroes)
    //    {
    //        if (!heroManager.ActiveHeroes.Contains(hero))
    //        {
    //            CreateHeroSlot(hero, allHeroesContainer, false);
    //        }
    //    }
    //}

    //private void CreateHeroSlot(Player hero, Transform container, bool isActive)
    //{
    //    GameObject slot = Instantiate(heroSlotPrefab, container);
    //    HeroSlotUI slotUI = slot.GetComponent<HeroSlotUI>();
    //    slotUI.SetHero(hero);
    //    slotUI.button.onClick.AddListener(() => OnHeroSlotClicked(hero, isActive));
    //}

    //private void OnHeroSlotClicked(Player hero, bool isActive)
    //{
    //    if (isActive)
    //    {
    //        heroManager.RemoveHeroFromPortal(hero);
    //    }
    //    else
    //    {
    //        heroManager.AddHeroToPortal(hero);
    //    }
    //    UpdateUI();
    //}
}