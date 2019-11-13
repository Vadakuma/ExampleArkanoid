using Arkanoid.GameStates;
using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.Commands
{
    /// <summary>
    /// pause by escape
    /// </summary>
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