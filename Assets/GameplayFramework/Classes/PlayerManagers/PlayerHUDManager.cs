namespace GameplayFramework
{
    public class PlayerHUDManager : PlayerManager
    {
        protected sealed override void AddTickHandler(TickHandler handler)
        {
            Game.TickPlayerHUDManagers += handler;
        }

        protected sealed override void RemoveTickHandler(TickHandler handler)
        {
            Game.TickPlayerHUDManagers -= handler;
        }



        protected override void Tick(TickArgs e)
        {
        }
    }
}