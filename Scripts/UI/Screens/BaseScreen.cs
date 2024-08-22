using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour
{
    [SerializeField] protected string _screenName;

    [SerializeField] protected MainUIManager _UIManager;

    // visual elements
    protected GameObject _screen;

    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(_screenName))
            _screenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        _screen = this.gameObject;
    }

    protected bool IsVisible()
    {
        if (_screen == null)
            return false;

        return (_screen.activeSelf);
    }

    // Toggle a UI on and off using the DisplayStyle. 
    private void SetObjectActivity(GameObject GO, bool state)
    {
        if (GO == null)
            return;

        GO.SetActive(state);
    }

    public virtual void ShowScreen()
    {
        if(!IsVisible())
        {
            SetObjectActivity(_screen, true);
        }
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            SetObjectActivity(_screen, false);
        }
    }
}
