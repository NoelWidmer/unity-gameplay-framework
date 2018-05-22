using System;

namespace GameplayFramework
{
    public abstract class Controller : IDisposable
    {
        public Controller()
        {
            TickEnabled = true;
        }



        private IPawn _pawn;

        public IPawn Pawn
        {
            get
            {
                return _pawn;
            }
        }



        #region Possess & UnPossess

        private readonly object _possessionLock = new object();



        public event EventHandler PossessedPawn;
        public event EventHandler UnPossessedPawn;



        public virtual void Possess(IPawn pawn)
        {
            if (pawn == null)
                throw new ArgumentNullException("pawn");

            lock (_possessionLock)
            {
                if (_pawn != null)
                    UnPossess();

                _pawn = pawn;
                pawn.OnBecamePossessed(this);

                EventHandler possessedPawn = PossessedPawn;
                if (possessedPawn != null)
                    possessedPawn(this, EventArgs.Empty);
            }
        }



        public virtual void UnPossess()
        {
            lock (_possessionLock)
            {
                if (_pawn == null)
                    return;

                _pawn = null;

                EventHandler unPossessedPawn = UnPossessedPawn;
                if (unPossessedPawn != null)
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
                if (value == _tickEnabled)
                    return;

                if (value)
                    Game.TickControllers += Tick;
                else
                    Game.TickControllers -= Tick;

                _tickEnabled = value;
            }
        }



        protected abstract void Tick(TickArgs e);



        public virtual void Dispose()
        {
            TickEnabled = false;
        }

        #endregion
    }
}