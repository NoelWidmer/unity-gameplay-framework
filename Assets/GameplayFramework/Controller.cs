using System;
using UnityEngine;

public abstract class Controller
{
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



    public virtual void Possess(Pawn pawn)
    {
        if(pawn == null)
            throw new ArgumentNullException();

        if(_pawn != null)
            UnPossess();

        _pawn = pawn;
        PossessedPawn.SafeInvoke(this, EventArgs.Empty);
    }



    public virtual void UnPossess()
    {
        if(_pawn == null)
            return;
        
        _pawn = null;
        UnPossessedPawn.SafeInvoke(this, EventArgs.Empty);
    }
}