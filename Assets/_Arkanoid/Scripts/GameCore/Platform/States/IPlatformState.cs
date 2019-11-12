using Arkanoid.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PlayerPlatform
{
    /// <summary>
    /// TODO: disposable
    /// </summary>
    public interface IPlatformState
    {
        void Init(Platform platform);
        void Update(Platform platform);
        void MoveTo(Platform platform, int side);
        void AddDamage(int amount);
        void AddAbility(Platform platform, IAbility ability);
        void Disable();
        void GoToNextState();
    }

}
