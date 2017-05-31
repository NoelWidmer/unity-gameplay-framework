namespace GameplayFramework
{
    public class PlayerInputManager : PlayerManager
    {
        protected sealed override void AddTickHandler(TickHandler handler)
        {
            Game.TickPlayerInput += handler;
        }

        protected sealed override void RemoveTickHandler(TickHandler handler)
        {
            Game.TickPlayerInput -= handler;
        }



        protected override void Tick(TickArgs e)
        {
        }
    }
}