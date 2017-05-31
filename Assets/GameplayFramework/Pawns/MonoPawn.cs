using System;
using UnityEngine;

namespace GameplayFramework
{
    public class MonoPawn : MonoBehaviour, IPawn, IDisposable
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



        public virtual void OnBecamePossessed(Controller controller)
        {
            if(controller == null)
                throw new ArgumentNullException("controller");

            lock(_lock)
            {
                if(_controller != null)
                {
                    if(_controller.Equals(controller))
                        return;

                    _controller.UnPossess();
                }

                _controller = controller;
                controller.UnPossessedPawn += (sender, e) => OnBecameUnPossessed();

                EventHandler becamePossessed = BecamePossessed;
                if(becamePossessed != null)
                    becamePossessed(this, EventArgs.Empty);
            }
        }



        protected virtual void OnBecameUnPossessed()
        {
            lock(_lock)
            {
                if(_controller == null)
                    return;
                
                _controller.UnPossessedPawn -= (sender, e) => OnBecameUnPossessed();
                _controller = null;

                EventHandler becameUnossessed = BecameUnPossessed;
                if(becameUnossessed != null)
                    becameUnossessed(this, EventArgs.Empty);
            }
        }



        public virtual void Dispose()
        {
            lock(_lock)
            {
                if(_controller != null)
                {
                    _controller.UnPossessedPawn -= (sender, e) => OnBecameUnPossessed();
                    _controller = null;
                }
            }
        }



        private void OnDestroy()
        {
            Dispose();
        }
    }
}