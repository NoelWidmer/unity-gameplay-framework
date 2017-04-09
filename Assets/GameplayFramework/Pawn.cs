using System;
using UnityEngine;

namespace GameplayFramework
{
    public class Pawn
    {
        private readonly object _lock = new object();
        private Controller _controller;

        public Controller Controller
        {
            get
            {
                return _controller;
            }
        }



        public event EventHandler BecamePossessed;
        public event EventHandler BecameUnPossessed;



        public void OnBecamePossessed(Controller controller)
        {
            if(controller == null)
                throw new ArgumentNullException("controller");

            lock(_lock)
            {
                if(_controller != null)
                {
                    if(_controller.GetHashCode() == controller.GetHashCode())
                        return;

                    _controller.UnPossess();
                }

                _controller = controller;

                var becamePossessed = BecamePossessed;
                if(becamePossessed != null)
                    becamePossessed(this, EventArgs.Empty);
            }
        }


        public void OnBecameUnPossessed()
        {
            lock(_lock)
            {
                if(_controller == null)
                    return;

                _controller = null;

                var becameUnossessed = BecameUnPossessed;
                if(becameUnossessed != null)
                    becameUnossessed(this, EventArgs.Empty);
            }
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