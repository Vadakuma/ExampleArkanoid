using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Commands
{
    /// <summary>
    /// moving to the right side
    /// </summary>
    public class RightWinCmd : Cmd
    {
        public RightWinCmd()
        {
            status = 1; // setup to the right moving
        }
    }
}