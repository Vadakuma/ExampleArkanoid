using Arkanoid.Enemies;
using Arkanoid.PickUps;
using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    public abstract class BaseGameState : IGameState
    {
        protected string stateName = "BaseGameState";

        public string StateName { get { return stateName; } protected set { stateName = value; } }

        public BaseGameState(IGameState prev)
        {
            SetStateName(); // set state name first of all

            if (prev != null)
            {
                prev.Disable();
            }

            if (GUIManager.Instance)
                GUIManager.Instance.SpawnMenu(StateName);

            SetDeActiveGameActors();
        }
        public virtual void Disable()
        {
            if (GUIManager.Instance)
                GUIManager.Instance.CloseMenu(StateName);
        }

        public virtual void Update() { }

        public void SetActiveGameActors()
        {
            // update value
            Ball.Instance.StartMoving();
            EnemyManager.Instance.UnPauseEnemies();
            PickUpManager.Instance.UnPauseSpawn();
            Platform.Instance.GoToActiveState();
        }

        protected virtual void SetStateName() { }

        public void SetDeActiveGameActors()
        {
            // update value
            Ball.Instance.StopMoving();
            EnemyManager.Instance.PauseEnemies();
            PickUpManager.Instance.PauseSpawn();
            Platform.Instance.GoToIdleState();
        }
    }
}