using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    /// <summary>
    /// TODO: add subscribing on LateUpdate, FixedUpdate
    /// </summary>
    public class UpdateManager : MonoBehaviour
    {
        private static event Action OnUpdate;
        private static event Action OnLateUpdate;
        private static event Action OnFixedUpdate;

        public static void SubscribeToFixedUpdate(Action e)
        {
            OnFixedUpdate -= e; // just in case if it the method called multiple times.
            OnFixedUpdate += e;
        }

        public static void UnSubscribeFromFixedUpdate(Action e)
        {
            OnFixedUpdate -= e;
        }

        public static void SubscribeToUpdate(Action e)
        {
            OnUpdate -= e; // just in case if it the method called multiple times.
            OnUpdate += e;
        }

        public static void UnSubscribeFromUpdate(Action e)
        {
            OnUpdate -= e;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void OnDestroy()
        {
            OnUpdate = null;
            OnLateUpdate = null;
            OnFixedUpdate = null;
        }
    }
}