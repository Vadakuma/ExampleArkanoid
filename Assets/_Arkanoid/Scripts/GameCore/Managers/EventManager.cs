using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Arkanoid
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<int, UnityEvent> eventDictionary;

        private static EventManager eventManager;

        public static EventManager instance
        {
            get
            {
                if (eventManager == null)
                {
                    eventManager = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
                    if (eventManager == null)
                    {
                        // TODO: refactoring
                        // it's ok. swears between levels loading
                        // Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    }
                    else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        private void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<int, UnityEvent>();
            }
        }

        /** */
        public static void StartListening(int eventName, UnityAction listener)
        {
            if (instance == null)
                return;

            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }


        public static void StopListening(int eventName, UnityAction listener)
        {
            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(int eventName)
        {
            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}
