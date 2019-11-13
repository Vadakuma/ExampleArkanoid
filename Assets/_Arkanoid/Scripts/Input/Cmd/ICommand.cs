using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.Commands
{

    public interface ICommand  { }

    /// <summary>
    /// Set up for list of commands from Platform
    /// </summary>
    public interface IPlatformCommand : ICommand
    {
        void execute(Platform mb);
    }
}