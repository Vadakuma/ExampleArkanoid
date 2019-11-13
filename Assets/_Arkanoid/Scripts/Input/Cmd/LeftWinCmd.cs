using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Commands
{
    /// <summary>
    /// moving to the left side 
    /// </summary>
    public class LeftWinCmd : Cmd
    {
        public LeftWinCmd()
        {
            status = -1; // setup to the left moving
        }
    }
}