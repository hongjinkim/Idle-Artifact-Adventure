//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TraitSelection : MonoBehaviour
//{
//    public int Level;
//    public GameObject buttonPrefab;
//    public Transform buttonContainer;

//    private List<Trait> availableTraits;
//    private List<Trait> selectedTraits = new List<Trait>();

//    void Start()
//    {
//        SelectTrait();
//    }

//    private void SelectTrait()
//    {
//        availableTraits = GetAvailableTraits(Level);

//        if (availableTraits.Count > 0)
//        {
//            foreach (var trait in availableTraits)
//            {
//                GameObject button = Instantiate(buttonPrefab, buttonContainer);
//                button.GetComponentInChildren<Text>().text = trait.Description;
//                button.GetComponent<Button>().onClick.AddListener(() => OnTraitSelected(trait));
//            }
//        }
//    }

//    private void OnTraitSelected(Trait selectedTrait)
//    {
//        selectedTraits.Add(selectedTrait);
//        selectedTrait.ApplyEffect();

//        // 선택 완료 후 버튼 비활성화
//        foreach (Transform child in buttonContainer)
//        {
//            Destroy(child.gameObject);
//        }
//    }

//    private List<Trait> GetAvailableTraits(int level)
//    {
//        List<Trait> traits = new List<Trait>();

//        // TraitType.Concentration
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "방어 침투를 3% 증가시킵니다.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Concentration, "피해를 3% 증폭시킵니다.", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "적을 5회 공격 후 후속 공격에 8%의 추가 피해를 입힙니다.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Concentration, "적을 5회 공격 후 3초 동안 20의 추가 공격 속도를 얻습니다.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "일반 공격은 타겟의 아머와 마법 저항을 6% 줄입니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Concentration, "일반 공격은 타겟의 체력 재생을 3초 동안 20으로 줄입니다.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "적을 죽이면 3초 동안 공격 속도가 25 증가합니다.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Concentration, "적을 죽이면 3초 동안 공격 피해가 12% 증가합니다.", () => { /* 공격 피해 증가 로직 */ }));
//        }

//        //Plunder
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "개인적인 경험치12% 추가.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Plunder, "팀 전체의 경험치 7% 추가.", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "아이템 드랍률이 2% 증가합니다.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Plunder, "골드 드랍률이 2% 증가합니다.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "장비 드랍률이 2% 증가합니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Plunder, "골드 드랍률이 2% 증가합니다.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "드랍되는 골드의 양이 10% 증가합니다.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Plunder, "보스는 항상 장비를 드랍합니다.", () => { /* 공격 피해 증가 로직 */ }));
//        }
//        //Plunder

//        //Magic
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Magic, "근처의 적을 죽일때 50 에너지 회복.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Magic, "일반 공격이 15 추가 에너지 생성.", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Magic, "일반 공격이 50% 해당하는 마법피해를 10% 확률로 입힘.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Magic, "아케인 유성(데미지30%)를 입힘.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Magic, "대상의 체력재생을 3초동안 25감소.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Magic, "스킬이 10%의 추가피해.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Magic, "궁극기 스킬은 500에너지(최대 에너지 20%)를 즉시 회복.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Magic, "궁극기 스킬을 캐스팅시 공격속도가 3초 동안 30증가.", () => { /* 공격 피해 증가 로직 */ }));
//        }
//        //Magic

//        //Protection
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Protection, "최대 HP를 3% 증가시킵니다.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Protection, "아머를 4% 증가시킵니다.", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Protection, "힘은 아머를 0.1증가.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Protection, "힘은 마법저항 0.1증가.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Protection, "5%의 적은 물리적 피해.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Protection, "5%의 적은 마법피해.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Protection, "근접공격에 피해 8%감소.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Protection, "원거리공격에 피해 8%감소.", () => { /* 공격 피해 증가 로직 */ }));
//        }
//        //Protection

//        //Life
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Life, "포션의 최대 소지량을 300 확장합니다.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Life, "포션 재사용 대기 시간을 10% 줄입니다.", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Life, "포션은 12% 더 많은 HP를 회복합니다.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Life, "체력 재생+7.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Life, "치명적인 피해를 입으면 최대 HP의 5% 회복합니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Life, "적을 물리치면 최대 HP의 3% 회복합니다.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Life, "???.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Life, "???.", () => { /* 공격 피해 증가 로직 */ }));
//        }
//        //Life

//        //Explosion
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "크리티컬 확률 +3%.", () => { /* 방어 침투 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Explosion, "크리티컬 데미지 +12%", () => { /* 피해 증폭 로직 */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "피해를 받을 때 6의 추가 에너지를 회복합니다.", () => { /* 추가 피해 로직 */ }));
//            traits.Add(new Trait(TraitType.Explosion, "영웅의 HP가 낮아질 수록 최대 8%의 추가 피해를 입힙니다.", () => { /* 추가 공격 속도 로직 */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "HP가 50%미만인 적에게 6%의 추가 피해를 입힙니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
//            traits.Add(new Trait(TraitType.Explosion, "HP가 50%이상인 적에게 6%의 추가 피해를 입힙니다.", () => { /* 체력 재생 감소 로직 */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "적을 죽이면 3초 동안 공격 속도가 25 증가합니다.", () => { /* 공격 속도 증가 로직 */ }));
//            traits.Add(new Trait(TraitType.Explosion, "적을 죽이면 3초 동안 공격 피해가 12% 증가합니다.", () => { /* 공격 피해 증가 로직 */ }));
//        }
//        //Explosion
//        return traits;
//    }
//}

