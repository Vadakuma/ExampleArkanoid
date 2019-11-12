using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PlayerPlatform
{
    /// <summary>
    /// Stop moving, get damage and ability restore data and postion
    /// </summary>
    public class ResetState : InitialState
    {
        public ResetState(Platform platform) : base(platform)
        {
            platform.transform.position = Platform.Initpos;
            platformSettings.Health = platformSettings.MaxHealth;
        }

        public new void Init(Platform platform) { }
        public new void Update(Platform platform)
        {
            if (parent)
                parent.GoToActiveState();
        }
        public new void MoveTo(Platform platform, int side) { }
    }

}
