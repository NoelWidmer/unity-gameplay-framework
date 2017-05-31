namespace GameplayFramework
{
    public class PlayerCameraManager : PlayerManager
    {
        public PlayerCameraManager(bool tickEnabled = true)
        {
            TickEnabled = tickEnabled;
        }



        protected sealed override void AddTickHandler(TickHandler handler)
        {
            Game.TickPlayerCamera += handler;
        }

        protected sealed override void RemoveTickHandler(TickHandler handler)
        {
            Game.TickPlayerCamera -= handler;
        }



        protected override void Tick(TickArgs e)
        {
        }
    }
}