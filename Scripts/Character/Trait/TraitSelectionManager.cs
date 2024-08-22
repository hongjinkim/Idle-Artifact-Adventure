//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//public class TraitSelectionManager : MonoBehaviour
//{
//    public Button[] initialTraitButtons; // 초기 특성 선택 버튼들
//    public GameObject[] traitUI; // 각 특성에 대한 UI 패널
//    public Button[][] levelButtons; // 각 특성의 레벨별 버튼 쌍
//    private TraitType selectedTrait; // 현재 선택된 특성
//    private bool traitSelected = false; // 특성이 선택되었는지 여부
//    private Player player; // 플레이어 참조

//    void Start()
//    {
//        player = FindObjectOfType<Player>(); // 씬에서 Player 객체 찾기
//        InitializeLevelButtons(); // 레벨 버튼 초기화
//        InitializeInitialTraitButtons(); // 초기 특성 버튼 초기화
//    }

//    void InitializeLevelButtons()
//    {
//        // TraitType enum의 개수만큼 2차원 배열 생성
//        levelButtons = new Button[System.Enum.GetValues(typeof(TraitType)).Length][];
//        for (int i = 0; i < levelButtons.Length; i++)
//        {
//            // 각 특성 UI에서 모든 버튼 컴포넌트를 가져와 할당
//            levelButtons[i] = traitUI[i].GetComponentsInChildren<Button>();
//        }
//    }

//    void InitializeInitialTraitButtons()
//    {
//        for (int i = 0; i < initialTraitButtons.Length; i++)
//        {
//            int index = i;
//            initialTraitButtons[i].onClick.AddListener(() => OnInitialTraitSelected(index));
//        }
//    }
//    public void OnInitialTraitSelected(int traitTypeIndex)
//    {
//        TraitType traitType = (TraitType)traitTypeIndex;

//        if (!traitSelected)
//        {
//            traitSelected = true;
//            selectedTrait = traitType;
//            traitUI[(int)selectedTrait].SetActive(true); // 선택된 특성 UI 활성화

//            // 선택된 버튼 외 모든 초기 특성 버튼 비활성화
//            foreach (Button button in initialTraitButtons)
//            {
//                button.interactable = button == initialTraitButtons[(int)traitType];
//            }

//            InitializeLevelButtonsForSelectedTrait();
//            OnLevelUp(player.Level); // 현재 플레이어 레벨에 맞게 버튼 활성화
//        }
//    }

//    void InitializeLevelButtonsForSelectedTrait()
//    {
//        // 선택된 특성의 레벨 버튼들에 대해 2개씩 짝지어 처리
//        for (int i = 0; i < levelButtons[(int)selectedTrait].Length; i += 2)
//        {
//            int index = i;
//            // 각 버튼 쌍에 클릭 이벤트 리스너 추가
//            levelButtons[(int)selectedTrait][i].onClick.AddListener(() => OnLevelTraitSelected(index));
//            levelButtons[(int)selectedTrait][i + 1].onClick.AddListener(() => OnLevelTraitSelected(index + 1));
//        }
//    }

//    void OnLevelTraitSelected(int buttonIndex)
//    {
//        // 선택된 버튼의 쌍 찾기
//        int pairStartIndex = (buttonIndex / 2) * 2;
//        // 해당 쌍의 두 버튼 모두 비활성화
//        levelButtons[(int)selectedTrait][pairStartIndex].interactable = false;
//        levelButtons[(int)selectedTrait][pairStartIndex + 1].interactable = false;
//        // 선택된 버튼만 다시 활성화
//        levelButtons[(int)selectedTrait][buttonIndex].interactable = true;
//    }

//    public void OnLevelUp(int level)
//    {
//        // 각 레벨 버튼 쌍에 대해
//        for (int i = 0; i < levelButtons[(int)selectedTrait].Length; i += 2)
//        {
//            int buttonLevel = (i / 2 + 1) * 10; // 버튼 레벨 계산 (10, 20, 30, 40)
//            bool shouldBeActive = level >= buttonLevel; // 활성화 여부 결정
//            // 레벨 조건을 만족하면 버튼 쌍 활성화
//            levelButtons[(int)selectedTrait][i].interactable = shouldBeActive;
//            levelButtons[(int)selectedTrait][i + 1].interactable = shouldBeActive;
//        }
//    }
//}