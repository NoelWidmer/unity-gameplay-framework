using System;
using UnityEngine;

namespace GameplayFramework
{
    /// <summary>
    ///     Local pawns are used for local play.
    /// </summary>
    public class MonoPawn : MonoBehaviour, IPawn, IDisposable
    {
        private readonly object _lock = new object();

        #region Implementation of IPawn

        private Controller _controller;
        /// <summary>
        ///     The controller that possesses this pawn or null if this pawn is unpossessed.
        /// </summary>
        public Controller Controller
        {
            get
            {
                return _controller;
            }
        }

        /// <summary>
        ///     Called when this pawn became possessed by a controller.
        /// </summary>
        public event EventHandler BecamePossessed;

        /// <summary>
        ///     Called when this pawn became unpossessed by a controller.
        /// </summary>
        public event EventHandler BecameUnPossessed;

        /// <summary>
        ///     Called by the controller when this pawn became possessed by it.
        /// </summary>
        /// <param name="controller">The controller that took possession of this pawn.</param>
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

        #endregion

        #region Implementation of IDisposable

        public virtual void Dispose()
        {
            lock(_lock)
            {
                if(_controller != null)
                    _controller.UnPossess();
            }
        }

        #endregion
        


        /// <summary>
        ///     This method is a subscriber to the <see cref="Controller"/>'s <see cref="Controller.UnPossessedPawn"/> event.
        /// </summary>
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

        private void OnDestroy()
        {
            Dispose();
        }
    }
}