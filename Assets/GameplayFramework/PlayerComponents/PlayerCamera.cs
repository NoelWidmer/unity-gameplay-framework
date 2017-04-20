using System;

namespace GameplayFramework
{
    public class PlayerCamera : IDisposable
    {
        public PlayerCamera(bool tickEnabled = true)
        {
            TickEnabled = tickEnabled;
        }



        private bool _tickEnabled = false;
        public bool TickEnabled
        {
            get
            {
                return _tickEnabled;
            }
            set
            {
                if(value == _tickEnabled)
                    return;

                if(value)
                {
                    World.TickPlayerCamera += Tick;
                }
                else
                {
                    World.TickPlayerCamera -= Tick;
                }

                _tickEnabled = value;
            }
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void Dispose()
        {
            TickEnabled = false;
        }
    }
}