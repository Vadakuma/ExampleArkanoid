using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PlayerPlatform
{

    /// <summary>
    /// MAIN GAME STATE Active input, set and get damage, set ability
    /// </summary>
    public class ActionState : InitialState
    {
        private Vector3 pos = Vector3.zero; // next platform position
        private Vector3 curpos = Vector3.zero; // next platform position
        private float platformShift;      // mavement shift amount
        private float nextpos;
        private int lastside = 0;

        public ActionState(Platform platform) : base(platform)
        {
            curpos = platform.transform.position; // set initial position
            pos = platform.transform.position; // set initial position

        }

        /** */
        public override void Update(Platform platform)
        {
            base.Update(platform); // base checking commands from input

            // moving the platform
            // TODO: clamping algorithm optimization need
            platformShift = Mathf.Lerp(platformShift, 0.0f, platformSettings.SpeedDampness * Time.deltaTime);
            if (platformShift != 0)
            {
                curpos = Vector3.Lerp(curpos, pos, platformSettings.SpeedDampness * Time.deltaTime);

                nextpos = Mathf.Clamp(curpos.x,
                     platformSettings.GetMovementShiftLimits.x,
                     platformSettings.GetMovementShiftLimits.y);
                curpos.x = nextpos;


                platform.transform.position = curpos;
            }
        }

        /// <summary>
        /// TODO: clamping algorithm optimization need
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="side"></param>
        public override void MoveTo(Platform platform, int side)
        {

            platformShift = side * platformSettings.Speed;

            // reset posx if switching movement direction 
            if (lastside != side)
            {
                lastside = side;
                // to save inversion effect we do this only on edges of level
                if (platform.transform.position.x == platformSettings.GetMovementShiftLimits.x ||
                   platform.transform.position.x == platformSettings.GetMovementShiftLimits.y)
                    pos.x = platform.transform.position.x;
            }

            pos.x = Mathf.Lerp(pos.x, platform.transform.position.x + platformShift, platformSettings.Acceleration * Time.deltaTime);
        }

        public override void AddDamage(int amount)
        {
            if (platformSettings.Health > 0)
            {
                platformSettings.Health -= amount;
                if (platformSettings.Health < 1)
                    parent.GoToDeadState();
            }
        }
    }
}
