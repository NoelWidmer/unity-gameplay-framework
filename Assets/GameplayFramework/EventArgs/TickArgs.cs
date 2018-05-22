using System;

namespace GameplayFramework
{
    public delegate void TickHandler(TickArgs e);



    public class TickArgs : EventArgs
    {
        public readonly float DeltaTime;

        public TickArgs(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}