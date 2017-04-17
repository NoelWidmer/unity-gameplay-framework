using System;

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



        public virtual void OnBecamePossessed(Controller controller)
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
                controller.UnPossessedPawn += (sender, e) => OnBecameUnPossessed();

                var becamePossessed = BecamePossessed;
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

                Controller oldController = _controller;
                _controller = null;
                _controller.UnPossessedPawn -= (sender, e) => OnBecameUnPossessed();

                var becameUnossessed = BecameUnPossessed;
                if(becameUnossessed != null)
                    becameUnossessed(this, EventArgs.Empty);
            }
        }
    }
}