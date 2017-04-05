using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        private PlayerInput _playerInput;
        private PlayerCamera _playerCamera;
        private PlayerHUD _playerHUD;

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
    }
}