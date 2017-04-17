using System;

namespace GameplayFramework
{
    public class PlayerInput : IDisposable
    {
        public PlayerInput(bool tickEnabled = true)
        {
            TickEnabled = tickEnabled;
        }



        private bool _tickEnabled = false;
        public virtual bool TickEnabled
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
                    Game.TickPlayerInput += Tick;
                }
                else
                {
                    Game.TickPlayerInput -= Tick;
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