namespace GameplayFramework
{
    public class GameMode
    {
        public GameMode()
        {
            Game.TickGameMode += Tick;
        }



        public virtual void BeginMode()
        {
            Game.Current.GameState = GetGameState();
        }



        public virtual void BeforeEndMode()
        {
        }



        public virtual void EndMode()
        {
            Game.TickGameMode -= Tick;
        }



        protected virtual GameState GetGameState()
        {
            return new GameState();
        }



        protected virtual void Tick(TickArgs e)
        {
        }
    }
}