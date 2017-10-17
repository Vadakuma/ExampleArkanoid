using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    public class UIMenu : MonoBehaviour
    {
        private static UIMenu _instance;

        public static UIMenu Instance { get { return _instance; } private set { _instance = value; } }
       
        // Use this for initialization
        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        void Start()
        {
    
        }
        // Update is called once per frame
        void Update()
        {

        }

        /** */
        public void ReturnToMainMenu() { }

        /** */
        public void GoToGame() {}

        /** */
        public void ApplicationQuit()
        {
            // need save data!

            Application.Quit();
        }
    }
}