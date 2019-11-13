using Arkanoid.Commands;
using Arkanoid.GameStates;
using Arkanoid.PlayerPlatform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.GameInput
{
    public class InputControl : IDisposable
    {
        //TODO: create dictionary with commands
        private Cmd toLeftCmd = new LeftWinCmd();
        private Cmd toRightCmd = new RightWinCmd();
        private Cmd toPauseCmd = new EscapeWinCmd();

        public delegate ICommand InpuUpdate();
        public event InpuUpdate Update = () => { return null; };


#if UNITY_ANDROID
        // unstead UI buttons 
        private readonly Rect leftHalfScreenSide = new Rect(0, 0, Screen.width/2, Screen.height / 2);
        private readonly Rect rightHalfScreenSide = new Rect(Screen.width / 2, 0, Screen.width/2, Screen.height / 2);
#endif

        // Use this for initialization
        public InputControl()
        {
            //check device platfrom
#if UNITY_STANDALONE
            Update += InputUpdater_STANDALONE;
#endif
#if UNITY_ANDROID
            Update += InputUpdater_ANDROID;
#endif
        }

        // Min updater. See Platform states Update method
        public ICommand InputUpdater()
        {
            return Update(); // return result depends on device platfrom 
        }

#if UNITY_STANDALONE
        /// <summary>
        /// BASE Windows Input stuff
        /// </summary>
        /// <returns></returns>
        private ICommand InputUpdater_STANDALONE()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                return toLeftCmd;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                return toRightCmd;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return toPauseCmd;
            }

            return null;
        }
#endif

#if UNITY_ANDROID
        /** BASE Android Input stuff
        It should work and for STANDALONE, but because using platform dependecy compilation is not.
        */
        private IPlatformCommand InputUpdater_ANDROID()
        {
            // for one touch controlling Mouse input enough instead using Mouse.Touches
            if (Input.GetMouseButton(0))
            {
                return CheckPosition(Input.mousePosition);
            }
            // Back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return toPauseCmd;
            }
            return null;
        }

        /** */
        private Cmd CheckPosition(Vector3 pos) {
            if (leftHalfScreenSide.Contains(pos))
            {
                return toLeftCmd;
            }

            if (rightHalfScreenSide.Contains(pos))
            {
                return toRightCmd;
            }

            return null;
        }
#endif

        /** */
        public void Dispose()
        {
            // remove event methods
#if UNITY_STANDALONE
            Update -= InputUpdater_STANDALONE;
#endif

#if UNITY_ANDROID
            Update -= InputUpdater_ANDROID;
#endif
        }
    }
}

    