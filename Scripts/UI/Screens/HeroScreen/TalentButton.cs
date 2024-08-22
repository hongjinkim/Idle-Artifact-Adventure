using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class TalentButton : MonoBehaviour
{
    [SerializeField] private Button mainButton;
    [SerializeField] private Button subButton;
    [SerializeField] private GameObject mainActivation;
    [SerializeField] private GameObject subActivation;

    private RectTransform mainButtonRect;
    private RectTransform subButtonRect;

    private void Awake()
    {
        mainButtonRect = mainButton.GetComponent<RectTransform>();
        subButtonRect = subButton.GetComponent<RectTransform>();

        mainButton.onClick.AddListener(OnMainButtonClick);
        subButton.onClick.AddListener(OnSubButtonClick);
    }
    private void Start()
    {
        mainButton.interactable = false;
        subButton.interactable = true;
    }

    private void OnMainButtonClick()
    {
        StartCoroutine(MoveButtonsRight());

        subActivation.SetActive(false);
        subButton.interactable = true;
        mainActivation.SetActive(true);
        mainButton.interactable = false;
    }

    private void OnSubButtonClick()
    {
        StartCoroutine(MoveButtonsLeft());

        mainActivation.SetActive(false);
        mainButton.interactable = true;
        subActivation.SetActive(true);
        subButton.interactable = false;
    }

    private IEnumerator MoveButtonsLeft()
    {
        Vector2 mainStartPosition = mainButtonRect.anchoredPosition;
        Vector2 subStartPosition = subButtonRect.anchoredPosition;
        float moveSpeed = 5f; 
        float mainTargetX = mainStartPosition.x - 200;
        float subTargetX = subStartPosition.x - 200;

        while (mainButtonRect.anchoredPosition.x > mainTargetX)
        {
            if (mainButtonRect.anchoredPosition.x > mainTargetX)
                mainButtonRect.anchoredPosition += new Vector2(-moveSpeed, 0f);
            if (subButtonRect.anchoredPosition.x > subTargetX)
                subButtonRect.anchoredPosition += new Vector2(-moveSpeed, 0f);
            yield return null;
        }
        mainButtonRect.anchoredPosition = new Vector2(mainTargetX, mainStartPosition.y);
        subButtonRect.anchoredPosition = new Vector2(subTargetX, subStartPosition.y);
    }

    private IEnumerator MoveButtonsRight()
    {
        Vector2 mainStartPosition = mainButtonRect.anchoredPosition;
        Vector2 subStartPosition = subButtonRect.anchoredPosition;
        float moveSpeed = 5f;
        float mainTargetX = mainStartPosition.x + 200;
        float subTargetX = subStartPosition.x + 200;

        while (mainButtonRect.anchoredPosition.x < mainTargetX)
        {
            if (mainButtonRect.anchoredPosition.x < mainTargetX)
                mainButtonRect.anchoredPosition += new Vector2(moveSpeed, 0f);
            if (subButtonRect.anchoredPosition.x < subTargetX)
                subButtonRect.anchoredPosition += new Vector2(moveSpeed, 0f);
            yield return null;
        }
        mainButtonRect.anchoredPosition = new Vector2(mainTargetX, mainStartPosition.y);
        subButtonRect.anchoredPosition = new Vector2(subTargetX, subStartPosition.y);
    }
}

