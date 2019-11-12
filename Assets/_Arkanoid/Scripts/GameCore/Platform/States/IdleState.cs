using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PlayerPlatform
{
    /// <summary>
    /// Stop moving, get damage and ability
    /// </summary>
    public class IdleState : InitialState
    {
        public IdleState(Platform platform) : base(platform) { }

        public new void Init(Platform platform) { }
        public new void Update(Platform platform) { }
        public new void MoveTo(Platform platform, int side) { }
    }
}
