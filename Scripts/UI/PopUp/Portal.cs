using UnityEngine.UI;
using UnityEngine;

public class Portal : PopUp
{
    [Header("Buttons")]
    public Button backButton;


    protected override void Start()
    {
        base.Start();
        backButton.onClick.AddListener(_UIManager.PortalPopUp.HideScreen);
    }
}
