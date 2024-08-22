using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : BaseScreen
{
    protected virtual void Start()
    {
        HideScreen();
    }

    public override void ShowScreen()
    {
        base.ShowScreen();
        transform.SetAsLastSibling();
    }

    public override void HideScreen()
    {
        base.HideScreen();
        transform.SetAsFirstSibling();
    }
}
