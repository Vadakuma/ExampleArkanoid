using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Arkanoid
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("Looking the string key to the game state names in GameState.cs and Listeners in InGameUI")]
        protected string menuName;
        // UICanvasContainer allow to easier make fadeInOut staff
        protected UICanvasContainer uICanvasContainer;

        public static bool isSetListeners = false;


        // Use this for initialization
        void Awake()
        {
            uICanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
               gameObject.GetComponent<CanvasGroupController>());

            uICanvasContainer.Fade(0.0f, 1.0f, false);
        }

        void OnEnable()  {       }
        void Start() {
            // set this ui menu to specials container InGameUI for controlling
            if (InGameUI.Instance)
                InGameUI.Instance.AddMenu(menuName, uICanvasContainer);

            AddListeners();
        }
        // Update is called once per frame
        void Update() {     }


        public static void AddListeners()
        {
            // set all listeners at once, it should work from any UIMenu 
            if (!isSetListeners)
            {
                //Debug.Log("AddListeners");
                isSetListeners = true;
                EventManager.StartListening("returntomenu", InGameUI.Instance.GoToMainMenu);
                EventManager.StartListening("resetround", GameState.Instance.GoToRestartLevelState);
                EventManager.StartListening("nextround", GameState.Instance.GoToGenerateLevelState);
                EventManager.StartListening("unpause", GameState.Instance.GoToPlayState);
                EventManager.StartListening("pause", GameState.Instance.GoToPauseState);
            }
        }

        public static void RemoveLesteners()
        {
            // remove all listeners at once
            if (isSetListeners)
            {
                //Debug.Log("RemoveLesteners");
                isSetListeners = false;
                // set all listeners at once we should work from any UIMenu 
                EventManager.StartListening("returntomenu", InGameUI.Instance.GoToMainMenu);
                EventManager.StartListening("resetround", GameState.Instance.GoToRestartLevelState);
                EventManager.StartListening("nextround", GameState.Instance.GoToGenerateLevelState);
                EventManager.StartListening("unpause", GameState.Instance.GoToPlayState);
                EventManager.StartListening("pause", GameState.Instance.GoToPauseState);
            }
        }

        /** call from buttons */
        public void OnClosePopup(string key)
        {
            EventManager.TriggerEvent(key);

            uICanvasContainer.Fade(0.0f, 0.1f, false);
        }

        void OnDisable()
        {
            RemoveLesteners();
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