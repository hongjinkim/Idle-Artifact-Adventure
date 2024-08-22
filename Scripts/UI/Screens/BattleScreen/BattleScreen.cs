using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScreen : MainScreen
{

    [Header("Object Pool")]
    public ObjectPoolBehaviour objectPool;
    private WaitForSeconds wait = new WaitForSeconds(2.0f);

    private void Start()
    {
        EventManager.StartListening(EventType.ItemPickup, OnPickup);
        EventManager.StartListening(EventType.DungeonEntered, ClearPickupNotice);
    }
    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventType.ItemPickup, OnPickup);
        EventManager.StopListening(EventType.DungeonEntered, ClearPickupNotice);
    }

    private void OnEnable()
    {
        SoundManager.Instance.EnableBattleSounds();
    }

    private void OnDisable()
    {
        SoundManager.Instance.DisableBattleSounds();
    }
    void OnPickup(Dictionary<string, object> message)
    {
        if(IsVisible())
        {
            if (message.ContainsKey("gold"))
            {
                StartCoroutine(PickupNotice((string)message["gold"]));
            }
            if (message.ContainsKey("item"))
            {
                if(message.ContainsKey("rarity"))
                    StartCoroutine(PickupNotice((string)message["item"], (ItemRarity)message["rarity"]));
                else
                    StartCoroutine(PickupNotice((string)message["item"]));
            }
        }
    }
    

    IEnumerator PickupNotice(string text)
    {
        GameObject textObject = objectPool.GetPooledObject();
        textObject.GetComponent<ItemPickupText>().SetText(text);
        textObject.SetActive(true);

        yield return wait;

        textObject.SetActive(false);
    }
    IEnumerator PickupNotice(string text, ItemRarity rarity)
    {
        GameObject textObject = objectPool.GetPooledObject();
        textObject.GetComponent<ItemPickupText>().SetText(text, rarity);
        textObject.SetActive(true);

        yield return wait;

        textObject.SetActive(false);
    }

    public void ClearPickupNotice(Dictionary<string, object> message)
    {
        objectPool.ClearObjects();
    }
}
