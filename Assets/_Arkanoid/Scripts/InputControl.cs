using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    public class InputControl
    {
        //TODO: create dictionary with commands
        private Cmd toLeftCmd = new LeftWinCmd();
        private Cmd toRightCmd = new RightWinCmd();
        private Cmd toPauseCmd = new EscapeWinCmd();

        public delegate IPlatformCommand InpuUpdate();
        public event InpuUpdate Update = () => { return null; };

        // Use this for initialization
        public InputControl()
        {
            //check device platfrom
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WebGLPlayer
                )
                Update += InputUpdater_WIN;
            else if (Application.platform == RuntimePlatform.Android)
                Update += InputUpdater_ANDROID;

            //Debug.Log("Application: " + Application.platform);
        }

        // Min updater. See Platform states Update method
        public IPlatformCommand InputUpdater()
        {
            return Update(); // return result depends on device platfrom 
        }

        /** BASE Windows Input staff*/
        private IPlatformCommand InputUpdater_WIN()
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

        /** BASE Android Input staff*/
        private IPlatformCommand InputUpdater_ANDROID()
        {
            return null;
        }


        public void OnDispose()
        {
            // remove event methods
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WebGLPlayer
                )
                Update -= InputUpdater_WIN;
            else if (Application.platform == RuntimePlatform.Android)
                Update -= InputUpdater_ANDROID;
        }
    }



    /** Set up for list of commands from Platform*/
    public interface IPlatformCommand
    {
        void execute(Platform mb);
    }

    /** BASE player platform movement class*/
    public abstract class Cmd : IPlatformCommand
    {
        protected int status;
        public virtual void execute(Platform pc)
        {
            if (pc)
            {
                pc.MoveTo(status);
            }
        }
    }

    /**  moving to the left side  */
    public class LeftWinCmd : Cmd
    {
        public LeftWinCmd()
        {
            status = -1; // setup to the left moving
        }
    }

    /** moving to the right side */
    public class RightWinCmd : Cmd
    {
        public RightWinCmd()
        {
            status = 1; // setup to the right moving
        }
    }


    /** pause by escape */
    public class EscapeWinCmd : Cmd
    {
        public override void execute(Platform pc)
        {
            if (GameState.Instance)
            {
                GameState.Instance.GoToPauseState();
            }
        }
    }
}

    