using System;

namespace GameplayFramework
{
    public abstract class Controller : IDisposable
    {
        public Controller()
        {
            Game.TickControllers += Tick;
        }

        private readonly object _possessionLock = new object();
        private Pawn _pawn;

        public Pawn Pawn
        {
            get
            {
                return _pawn;
            }
        }



        public event EventHandler PossessedPawn;
        public event EventHandler UnPossessedPawn;



        public void Possess(Pawn pawn)
        {
            if(pawn == null)
                throw new ArgumentNullException("pawn");

            lock(_possessionLock)
            {
                if(_pawn != null)
                    UnPossess();

                _pawn = pawn;
                pawn.OnBecamePossessed(this);

                var possessedPawn = PossessedPawn;
                if(possessedPawn != null)
                    possessedPawn(this, EventArgs.Empty);
            }
        }



        public void UnPossess()
        {
            lock(_possessionLock)
            {
                if(_pawn == null)
                    return;
                
                _pawn = null;

                var unPossessedPawn = UnPossessedPawn;
                if(unPossessedPawn != null)
                    unPossessedPawn(this, EventArgs.Empty);
            }
        }



        protected virtual void Tick(TickArgs e)
        {
        }



        public virtual void Dispose()
        {
            Game.TickControllers -= Tick;
        }
    }
}