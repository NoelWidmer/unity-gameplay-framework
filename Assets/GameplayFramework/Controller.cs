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



        public event EventHandler<EventArgs> PossessedPawn;
        public event EventHandler<EventArgs> UnPossessedPawn;



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
                PossessedPawn.SafeInvoke(this, EventArgs.Empty);
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
                UnPossessedPawn.SafeInvoke(this, EventArgs.Empty);
            }
        }



        public virtual void Tick(float deltaTime)
        {

        }
    }
}