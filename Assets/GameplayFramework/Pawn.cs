using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayFramework
{
    public class Pawn
    {
        private Controller _controller;

        public Controller Controller
        {
            get
            {
                return _controller;
            }
        }



        public event EventHandler<EventArgs> BecamePossessed;
        public event EventHandler<EventArgs> BecameUnPossessed;



        public void OnBecamePossessed(Controller controller)
        {
            if(controller == null)
                throw new ArgumentNullException();

            if(_controller != null)
            {
                if(_controller.GetHashCode() == controller.GetHashCode())
                    return;

                _controller.UnPossess();
            }

            _controller = controller;
            BecamePossessed.SafeInvoke(this, EventArgs.Empty);
        }


        public void OnBecameUnPossessed()
        {
            if(_controller == null)
                return;

            _controller = null;
            BecameUnPossessed.SafeInvoke(this, EventArgs.Empty);
        }



        protected virtual void TriggeredByUpdateSomehow(float deltaTime)
        {
            Controller controller = _controller;
            if(controller != null && controller is PlayerController)
            {
                PlayerController playerController = (PlayerController)controller;

                playerController.PlayerInput.Tick(deltaTime);
                playerController.Tick(deltaTime);
            }
        }



        protected virtual void TriggeredByLateUpdateSomehow(float deltaTime)
        {
            Controller controller = _controller;
            if(controller != null && controller is PlayerController)
            {
                PlayerController playerController = (PlayerController)controller;

                playerController.PlayerCamera.Tick(deltaTime);
                playerController.PlayerHUD.Tick(deltaTime);
            }
        }
    }
}