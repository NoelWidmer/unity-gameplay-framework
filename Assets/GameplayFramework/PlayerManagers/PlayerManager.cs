using System;
using GameplayFramework;

public abstract class PlayerManager : IDisposable
{
    private readonly object _tickLock = new object();



    private bool _tickEnabled = false;
    public bool TickEnabled
    {
        get
        {
            return _tickEnabled;
        }
        set
        {
            lock(_tickLock)
            {
                if(value == _tickEnabled)
                    return;

                if(value)
                    AddTickHandler(Tick);
                else
                    RemoveTickHandler(Tick);

                _tickEnabled = value;
            }
        }
    }



    protected abstract void AddTickHandler(TickHandler handler);
    protected abstract void RemoveTickHandler(TickHandler handler);



    protected abstract void Tick(TickArgs e);



    public virtual void Dispose()
    {
        TickEnabled = false;
    }
}