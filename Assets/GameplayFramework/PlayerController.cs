using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        private bool _initialized = false;

        private PlayerInput _playerInput;
        private PlayerCamera _playerCamera;
        private PlayerHUD _playerHUD;



        public PlayerController()
        {
            CallInitialize();
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



        protected virtual void CallInitialize()
        {
            Initialize<PlayerInput, PlayerCamera, PlayerHUD>();
        }



        protected void Initialize<TPlayerInput, TPlayerCamera, TPlayerHUD>() 
            where TPlayerInput : PlayerInput, new()
            where TPlayerCamera : PlayerCamera, new()
            where TPlayerHUD : PlayerHUD, new()
        {
            if(_initialized)
                throw new InvalidOperationException();

            _playerInput = new TPlayerInput();
            _playerCamera = new TPlayerCamera();
            _playerHUD = new TPlayerHUD();

            _initialized = true;
        }
    }
}