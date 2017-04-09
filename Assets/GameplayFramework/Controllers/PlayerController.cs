using System;
using UnityEngine;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        private bool _playerComponentsInstantiated;

        private PlayerInput _playerInput;
        private PlayerCamera _playerCamera;
        private PlayerHUD _playerHUD;



        public PlayerController()
        {
            Initialize();

            if(_playerComponentsInstantiated == false)
                throw new InvalidOperationException("The player's components have not been set.");
        }



        public PlayerInput PlayerInput
        {
            get
            {
                return _playerInput;
            }
        }

        public PlayerCamera PlayerCamera
        {
            get
            {
                return _playerCamera;
            }
        }

        public PlayerHUD PlayerHUD
        {
            get
            {
                return _playerHUD;
            }
        }



        protected virtual void Initialize()
        {
            InstantiatePlayerComponents<PlayerInput, PlayerCamera, PlayerHUD>();
        }



        protected void InstantiatePlayerComponents<TPlayerInput, TPlayerCamera, TPlayerHUD>() 
            where TPlayerInput : PlayerInput, new()
            where TPlayerCamera : PlayerCamera, new()
            where TPlayerHUD : PlayerHUD, new()
        {
            if(_playerComponentsInstantiated)
                throw new InvalidOperationException("Player components have already been initialized.");

            _playerInput = new TPlayerInput();
            _playerCamera = new TPlayerCamera();
            _playerHUD = new TPlayerHUD();

            _playerComponentsInstantiated = true;
        }
    }
}