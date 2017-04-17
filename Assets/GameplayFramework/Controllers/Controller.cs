using System;

namespace GameplayFramework
{
    public abstract class Controller : IDisposable
    {
        public Controller()
        {
            TickEnabled = true;
        }


        
        public Pawn Pawn
        {
            get;
            protected set;
        }

        #region Possess & UnPossess

        private readonly object _possessionLock = new object();

        public event EventHandler PossessedPawn;
        public event EventHandler UnPossessedPawn;



        public virtual void Possess(Pawn pawn)
        {
            if(pawn == null)
                throw new ArgumentNullException("pawn");

            lock(_possessionLock)
            {
                if(Pawn != null)
                    UnPossess();

                Pawn = pawn;
                pawn.OnBecamePossessed(this);

                var possessedPawn = PossessedPawn;
                if(possessedPawn != null)
                    possessedPawn(this, EventArgs.Empty);
            }
        }



        public virtual void UnPossess()
        {
            lock(_possessionLock)
            {
                if(Pawn == null)
                    return;
                
                Pawn = null;

                var unPossessedPawn = UnPossessedPawn;
                if(unPossessedPawn != null)
                    unPossessedPawn(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Tick

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
                    Game.TickControllers += Tick;
                }
                else
                {
                    Game.TickControllers -= Tick;
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

        #endregion
    }
}