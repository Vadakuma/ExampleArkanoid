using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    /** show info to user interface*/
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        protected Text hightscore;
        [SerializeField]
        protected Text roundindex;

        // Use this for initialization
        void Start()
        {
            // load data
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}