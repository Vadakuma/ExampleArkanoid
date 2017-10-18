using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class UIMenu : MonoBehaviour
    {
        [SerializeField, Tooltip("Looking to the game state names in GameState.cs")]
        protected string menuName;

        //private static UIMenu _instance;
        //public  static UIMenu Instance { get { return _instance; } private set { _instance = value; } }

        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer uICanvasContainer = new UICanvasContainer();

        // Use this for initialization
        void Awake()
        {
            //Instance = this;
            uICanvasContainer.cgroup = gameObject.GetComponent<CanvasGroup>();
            uICanvasContainer.cgcontroller = gameObject.GetComponent<CanvasGroupController>();
            uICanvasContainer.Restart(0.0f, 1.0f, false);
        }


        void Start()
        {
            // set this ui menu to specials container InGameUI for controlling
            if (InGameUI.Instance)
                InGameUI.Instance.AddMenu(menuName, uICanvasContainer);
        }

        // Update is called once per frame
        void Update() {     }

        /** */
        public void OnClosePopup(string key)
        {
            switch(key)
            {
                case "startround":
                    break;
                case "returntomenu":
                    SceneLoad sceneLoad = new SceneLoad();
                    sceneLoad.Load(0);
                    break;
                case "restart":
                    break;
                case "unpause":
                    break;
                case "nextround":
                    break;
                case "resetround":
                    break;
            }

            uICanvasContainer.Restart(0.0f, 0.1f, false);
        }

    }


    // UICanvasContainer allow to easier make fadeInOut staff
    [System.Serializable]
    public class UICanvasContainer
    {
        // menu canvas group
        public CanvasGroup cgroup;
        // special controller for alpha from canvas group
        public CanvasGroupController cgcontroller;

        public void Restart(float _alpha, float _smoothness, bool _destroy)
        {
            if(cgcontroller != null && cgroup != null)
             cgcontroller.Restart(cgroup, _alpha, _smoothness, _destroy);
        }
    }
}