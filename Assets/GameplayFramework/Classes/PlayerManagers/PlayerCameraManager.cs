namespace GameplayFramework
{
    public class PlayerCameraManager : PlayerManager
    {
        protected sealed override void AddTickHandler(TickHandler handler)
        {
            Game.TickPlayerCameraManagers += handler;
        }

        protected sealed override void RemoveTickHandler(TickHandler handler)
        {
            Game.TickPlayerCameraManagers -= handler;
        }



        protected override void Tick(TickArgs e)
        {
        }
    }
}