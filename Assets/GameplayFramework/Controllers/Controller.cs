using System;
using UnityEngine;

namespace GameplayFramework
{
    public abstract class Controller
    {
        private readonly object _lock = new object();
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
                throw new ArgumentNullException();

            lock(_lock)
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
            lock(_lock)
            {
                if(_pawn == null)
                    return;

                Pawn pawn = _pawn;
                _pawn = null;

                pawn.OnBecameUnPossessed();

                var unPossessedPawn = UnPossessedPawn;
                if(unPossessedPawn != null)
                    unPossessedPawn(this, EventArgs.Empty);
            }
        }



        public virtual void Tick(float deltaTime)
        {
        }
    }
}