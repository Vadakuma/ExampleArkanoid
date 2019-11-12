using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.GameStates
{
    /// <summary>
    /// Dispose!!!
    /// </summary>
    public interface IGameState
    {
        void Disable();
        void Update();
    }

}