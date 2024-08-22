using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class TutorialWithUIInteraction : PopUp
{
    [SerializeField] private GameObject[] tutorialObjects;
    [SerializeField] private Button[] tutorialButtons;
    [SerializeField] private Button[] autoClickButtons;
    private int currentTutorialIndex = -1;
    protected override void Start()
    {
        if (!GameDataManager.instance.playerInfo.isTutorial)
        {
            ActivateNextTutorial();
        }
        if (tutorialButtons != null)
        {
            for (int i = 0; i < tutorialButtons.Length; i++)
            {
                int index = i; // 클로저 문제를 피하기 위해 로컬 변수 사용
                tutorialButtons[i].onClick.AddListener(() => StartCoroutine(HandleClick()));
            }
        }
    }
    private IEnumerator HandleClick()
    {
        yield return new WaitForEndOfFrame();
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            ActivateNextTutorial();
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            ActivateNextTutorial();
        }
    }
    private void ActivateNextTutorial()
    {
        if (tutorialObjects == null || tutorialObjects.Length == 0)
        {
            return;
        }
        if (currentTutorialIndex >= 0 && currentTutorialIndex < tutorialObjects.Length)
        {
            tutorialObjects[currentTutorialIndex].SetActive(false);
        }
        currentTutorialIndex++;
        if (currentTutorialIndex < tutorialObjects.Length)
        {
            tutorialObjects[currentTutorialIndex].SetActive(true);
            if (currentTutorialIndex == 1)
            {
                AutoClickButton(1);
            }
            else if (currentTutorialIndex == 2)
            {
                AutoClickButton(2);
            }
            else if (currentTutorialIndex == 3)
            {
                AutoClickButton(3);
            }
            else if (currentTutorialIndex == 4)
            {
                AutoClickButton(4);
            }
            else if (currentTutorialIndex == 5)
            {
                AutoClickButton(5);
            }
            else if (currentTutorialIndex == 6)
            {
                AutoClickButton(6);
            }
            ShowScreen();
        }
        else
        {
            HideScreen();
            GameDataManager.instance.playerInfo.isTutorial = true;
        }
    }
    private void AutoClickButton(int buttonIndex)
    {
        if (autoClickButtons != null && buttonIndex >= 0 && buttonIndex < autoClickButtons.Length)
        {
            autoClickButtons[buttonIndex].onClick.Invoke();
        }
    }
}