using System;



public delegate void TickHandler(TickArgs e);



public class TickArgs : EventArgs
{
    private readonly float _deltaTime;

    public TickArgs(float deltaTime)
    {
        _deltaTime = deltaTime;
    }



    public float DeltaTime
    {
        get
        {
            return _deltaTime;
        }
    }
}