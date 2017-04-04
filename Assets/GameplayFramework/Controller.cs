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



    public event EventHandler PossessedPawn;
    public event EventHandler UnPossessedPawn;



    public virtual void Possess(Pawn pawn)
    {
        if(pawn == null)
            throw new ArgumentNullException();

        if(_pawn != null)
            UnPossess();

        _pawn = pawn;

        EventHandler subscribers = PossessedPawn;
        if(subscribers != null)
            subscribers(this, EventArgs.Empty);
    }



    public virtual void UnPossess()
    {
        if(_pawn == null)
            return;
        
        _pawn = null;

        EventHandler subscribers = UnPossessedPawn;
        if(subscribers != null)
            subscribers(this, EventArgs.Empty);
    }
}