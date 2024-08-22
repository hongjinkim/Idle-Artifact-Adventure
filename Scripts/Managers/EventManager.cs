using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<EventType, Action<Dictionary<string, object>>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();

                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(eventManager);
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventType, Action<Dictionary<string, object>>>();
        }
    }

    public static void StartListening(EventType eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventType eventName, Action<Dictionary<string, object>> listener)
    {
        if (eventManager == null) return;
        Action<Dictionary<string, object>> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(EventType eventName, Dictionary<string, object> message)
    {
        Action<Dictionary<string, object>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}

//사용 예시

//public class Producer : MonoBehaviour
//{
//    void OnTriggerEnter2D(Collider2D other)
//    {
//        EventManager.TriggerEvent("addCoins", new Dictionary<string, object> { { "amount", 1 } });
//    }
//}

//public class Consumer : MonoBehaviour
//{
//    private int coins;
//    void Start()
//    {
//        EventManager.StartListening("addCoins", OnAddCoins);
//    }
//    void OnApplicationQuit()
//    {
//        EventManager.StopListening("addCoins", OnAddCoins);
//    }
//    void OnAddcoins(Dictionary<string, object> message)
//    {
//        var amount = (int)message["amount"];
//        coins += amount;
//    }
//}