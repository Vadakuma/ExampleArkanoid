using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Commands
{
    /// <summary>
    /// BASE player platform movement class
    /// </summary>
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
}