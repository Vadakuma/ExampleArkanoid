using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.LevelGenerator
{
    public interface ILevelGenerator
    {
        LevelSettings Generate();
        void ResetLevel(LevelSettings _ls);
    }
}