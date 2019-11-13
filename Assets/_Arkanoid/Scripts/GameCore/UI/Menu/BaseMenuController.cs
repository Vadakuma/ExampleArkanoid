using Arkanoid.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Arkanoid.UI
{
    public abstract class BaseMenuController : MonoBehaviour
    {
        public enum UICommands
        {
            UIC_RETURNTOMENU = 0,
            UIC_RESETROUND = 1,
            UIC_NEXTROUND = 2,
            UIC_UNPAUSE = 3,
            UIC_PAUSE = 4
        };


        [SerializeField, Tooltip("Looking the string key to the game state names in GameState.cs and Listeners in InGameUI")]
        protected string            menuName;
        // UICanvasContainer allow to easier make fadeInOut staff
        protected UICanvasContainer uICanvasContainer;

        public static bool          IsSetListeners = false;

        // Use this for initialization
        private void Awake()
        {
            uICanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
               gameObject.GetComponent<CanvasGroupController>());

            uICanvasContainer.Fade(0.0f, 1.0f, false);
        }

        private void Start()
        {
            // set this ui menu to specials container InGameUI for controlling
            if (GUIManager.Instance)
                GUIManager.Instance.AddMenu(menuName, uICanvasContainer);

            AddListeners();
        }

        /// <summary>
        /// set listeners for menu buttons event
        /// </summary>
        protected virtual void AddListeners()
        {
            // set all listeners at once, it should work from any UIMenu 
            if (!IsSetListeners)
            {
                //Debug.Log("AddListeners");
                IsSetListeners = true;
                EventManager.StartListening((int)UICommands.UIC_RETURNTOMENU, GUIManager.Instance.GoToMainMenu);
                EventManager.StartListening((int)UICommands.UIC_RESETROUND, GameState.Instance.GoToRestartLevelState);
                EventManager.StartListening((int)UICommands.UIC_NEXTROUND, GameState.Instance.GoToGenerateLevelState);
                EventManager.StartListening((int)UICommands.UIC_UNPAUSE, GameState.Instance.GoToPlayState);
                EventManager.StartListening((int)UICommands.UIC_PAUSE, GameState.Instance.GoToPauseState);
            }
        }

        /// <summary>
        /// remove all listeners
        /// </summary>
        protected virtual void RemoveListeners()
        {
            // remove all listeners at once
            if (IsSetListeners)
            {
                //Debug.Log("RemoveLesteners");
                IsSetListeners = false;
                EventManager.StopListening((int)UICommands.UIC_RETURNTOMENU, GUIManager.Instance.GoToMainMenu);
                EventManager.StopListening((int)UICommands.UIC_RESETROUND, GameState.Instance.GoToRestartLevelState);
                EventManager.StopListening((int)UICommands.UIC_NEXTROUND, GameState.Instance.GoToGenerateLevelState);
                EventManager.StopListening((int)UICommands.UIC_UNPAUSE, GameState.Instance.GoToPlayState);
                EventManager.StopListening((int)UICommands.UIC_PAUSE, GameState.Instance.GoToPauseState);
            }
        }

        /// <summary>
        /// Button event. Call right method by TriggerEvent key
        /// </summary>
        /// <param name="key"></param>
        public virtual void OnClosePopup(int key)
        {
            EventManager.TriggerEvent(key);

            uICanvasContainer.Fade(0.0f, 0.1f, false);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }


}