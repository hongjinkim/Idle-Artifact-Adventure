using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Main")]
    public Image icon;
    public Image background;
    public Image frame;
    public Text count;
    public Toggle toggle;

    [Header("Extra")]
    public Sprite iconEmpty;
    public Sprite iconMissed;
    public Image comparer;
    public Image fragment;
    public GameObject modificator;
    public Text modificatorText;

    public Item item { get; private set; }

    private Action _scheduled;
    private float _clickTime;

    /// <summary>
    /// These actions should be set when inventory UI is opened.
    /// </summary>
    public static Action<Item> OnLeftClick;
    public static Action<Item> OnRightClick;
    public static Action<Item> OnDoubleClick;
    public static Action<Item> OnMouseEnter;
    public static Action<Item> OnMouseExit;

    public void OnEnable()
    {
        if (_scheduled != null)
        {
            StartCoroutine(ExecuteScheduled());
        }
    }

    public void Initialize(Item item, ToggleGroup toggleGroup = null)
    {
        this.item = item;

        if (this.item == null)
        {
            Reset();
            return;
        }

        icon.enabled = true;
        icon.sprite = item.id == null ? iconEmpty : ItemCollection.active.GetItemIcon(this.item)?.sprite ?? iconMissed;
        background.sprite = ItemCollection.active.GetBackground(this.item) ?? ItemCollection.active.backgroundBrown;
        background.color = Color.white;
        frame.raycastTarget = true;

        if (count)
        {
            count.SetActive(true);
            count.text = item.count.ToString();
        }

        //if (fragment)
        //{
        //    fragment.SetActive(true);
        //    fragment.SetActive(this.item.Params.Type == ItemType.Fragment);
        //}

        if (toggle)
        {
            toggle.group = toggleGroup ?? GetComponentInParent<ToggleGroup>();
        }

        if (modificator)
        {
            var mod = this.item.modifier != null ;

            modificator.SetActive(mod);

            if (mod)
            {
                string text;

                switch (this.item.modifier.id)
                {
                    //case MagicStat.LevelDown: text = "G-"; break;
                    //case MagicStat.LevelUp: text = "G+"; break;
                    default: text = this.item.modifier.id.ToString().ToUpper()[0].ToString(); break;
                }

                modificatorText.text = text;
            }
        }
    }

    public void Reset()
    {
        icon.enabled = false;
        icon.sprite = null;
        background.sprite = ItemCollection.active.backgroundBrown ?? background.sprite;
        background.color = new Color32(150, 150, 150, 255);
        frame.raycastTarget = false;

        if (count) count.SetActive(false);
        if (toggle) { toggle.group = null; toggle.SetIsOnWithoutNotify(false); }
        if (modificator) modificator.SetActive(false);
        if (comparer) comparer.SetActive(false);
        if (fragment) fragment.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick(eventData.button);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(item);
    }

    public void OnPointerClick(PointerEventData.InputButton button)
    {
        if (button == PointerEventData.InputButton.Left)
        {
            OnLeftClick?.Invoke(item);

            var delta = Mathf.Abs(Time.time - _clickTime);

            if (delta < 0.5f) // If double click.
            {
                _clickTime = 0;

                if (OnDoubleClick != null)
                {
                    StartCoroutine(ExecuteInNextUpdate(() => OnDoubleClick(item)));
                }
            }
            else
            {
                _clickTime = Time.time;
            }
        }
        else if (button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke(item);
        }
    }

    private static IEnumerator ExecuteInNextUpdate(Action action)
    {
        yield return null;

        action();
    }

    public void Select(bool selected)
    {
        if (toggle == null) return;

        if (gameObject.activeInHierarchy || !selected)
        {
            toggle.isOn = selected;
        }
        else
        {
            _scheduled = () => toggle.isOn = true;
        }

        if (selected)
        {
            OnLeftClick?.Invoke(item);
        }
    }

    private IEnumerator ExecuteScheduled()
    {
        yield return null;

        _scheduled();
        _scheduled = null;
    }
}
