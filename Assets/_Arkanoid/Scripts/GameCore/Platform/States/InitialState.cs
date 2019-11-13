using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.Abilities;
using Arkanoid.Commands;
using Arkanoid.GameInput;

namespace Arkanoid.PlayerPlatform
{
    public abstract class InitialState : IPlatformState
    {
        // link to current tick command
        private ICommand command;
        // 
        private InputControl inputcontrol;
        // base settings about movement and health
        protected PlatformSettings platformSettings;
        protected Platform parent;

        public InitialState(Platform platform)
        {
            Init(platform); // setup settings
        }

        public virtual void Init(Platform platform)
        {
            parent = platform;
            platformSettings = platform.GetPlatformSettings;
            inputcontrol = new InputControl();
        }

        public virtual void MoveTo(Platform platform, int side) { }

        public virtual void Update(Platform platform)
        {

            // checking and  get command from input control
            command = inputcontrol.InputUpdater();
            if (command != null)
            {
                command.execute(platform);
            }
        }

        public virtual void AddDamage(int amount) { }

        public void AddAbility(Platform platform, IAbility ability)
        {
            try
            {
                ability.Apply(platform);
            }
            catch
            {

            }
        }

        public void GoToNextState() { }

        public void Disable()
        {
            inputcontrol.Dispose();
        }
    }

}
