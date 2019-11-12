using Arkanoid.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Arkanoid
{
    public abstract class GUIMenu : MonoBehaviour
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

        public static bool          isSetListeners = false;

        // Use this for initialization
        void Awake()
        {
            uICanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
               gameObject.GetComponent<CanvasGroupController>());

            uICanvasContainer.Fade(0.0f, 1.0f, false);
        }

        void Start() {
            // set this ui menu to specials container InGameUI for controlling
            if (GUIManager.Instance)
                GUIManager.Instance.AddMenu(menuName, uICanvasContainer);

            AddListeners();
        }
        // Update is called once per frame
        void Update() {     }

        /** set listeners for menu buttons event*/
        protected virtual void AddListeners()
        {
            // set all listeners at once, it should work from any UIMenu 
            if (!isSetListeners)
            {
                //Debug.Log("AddListeners");
                isSetListeners = true;
                EventManager.StartListening((int)UICommands.UIC_RETURNTOMENU, GUIManager.Instance.GoToMainMenu);
                EventManager.StartListening((int)UICommands.UIC_RESETROUND, GameState.Instance.GoToRestartLevelState);
                EventManager.StartListening((int)UICommands.UIC_NEXTROUND, GameState.Instance.GoToGenerateLevelState);
                EventManager.StartListening((int)UICommands.UIC_UNPAUSE, GameState.Instance.GoToPlayState);
                EventManager.StartListening((int)UICommands.UIC_PAUSE, GameState.Instance.GoToPauseState);
            }
        }

        /** remove all listeners */
        protected virtual void RemoveListeners()
        {
            // remove all listeners at once
            if (isSetListeners)
            {
                //Debug.Log("RemoveLesteners");
                isSetListeners = false;
                EventManager.StopListening((int)UICommands.UIC_RETURNTOMENU, GUIManager.Instance.GoToMainMenu);
                EventManager.StopListening((int)UICommands.UIC_RESETROUND, GameState.Instance.GoToRestartLevelState);
                EventManager.StopListening((int)UICommands.UIC_NEXTROUND, GameState.Instance.GoToGenerateLevelState);
                EventManager.StopListening((int)UICommands.UIC_UNPAUSE, GameState.Instance.GoToPlayState);
                EventManager.StopListening((int)UICommands.UIC_PAUSE, GameState.Instance.GoToPauseState);
            }
        }

        /** Button event. Call right method by TriggerEvent key  */
        public virtual void OnClosePopup(int key)
        {
            EventManager.TriggerEvent(key);

            uICanvasContainer.Fade(0.0f, 0.1f, false);
        }

        void OnDisable()
        {
            RemoveListeners();
        }
    }

    // UICanvasContainer allow to easier make fadeInOut staff
    [System.Serializable]
    public class UICanvasContainer
    {
        // menu canvas group
        public CanvasGroup              cgroup;
        // special controller for alpha from canvas group
        public CanvasGroupController    cgcontroller;

        public UICanvasContainer() { }
        public UICanvasContainer(CanvasGroup cg, CanvasGroupController cgc)
        {
            cgroup = cg;
            cgcontroller = cgc;
        }

        /**
		* _alpha - current value in alpha field
		* _smoothness - lerp smoothnes on changing 
		* _destroy - destroy CanvasGroupController at the end
		*/
        public void Fade(float _alpha, float _smoothness, bool _destroy)
        {
            if(cgcontroller != null && cgroup != null)
                cgcontroller.Fade(cgroup, _alpha, _smoothness, _destroy);
        }


        /** apply some canvas group params. Fields from CanvasGroup component
         * _alpha -
         * _blockraycasts -
         * _interactable -
         */
        public void UpdateCanvasGroup(float _alpha, bool _blockraycasts, bool _interactable)
        {
            if (cgroup)
            {
                cgroup.alpha = _alpha;
                cgroup.blocksRaycasts = _blockraycasts;
                cgroup.interactable = _interactable;
            }
        }
    }
}