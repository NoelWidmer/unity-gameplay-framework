namespace GameplayFramework
{
    public class PlayerHUDManager : PlayerManager
    {
        public PlayerHUDManager(bool tickEnabled = true)
        {
            TickEnabled = tickEnabled;
        }



        protected sealed override void AddTickHandler(TickHandler handler)
        {
            Game.TickPlayerHUD += handler;
        }

        protected sealed override void RemoveTickHandler(TickHandler handler)
        {
            Game.TickPlayerHUD -= handler;
        }



        protected override void Tick(TickArgs e)
        {
        }
    }
}