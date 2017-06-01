namespace GameplayFramework.Sample
{
    public class SampleGameMode : GameMode
    {
        protected override void SetGameState()
        {
            Game.GameState = new SampleGameState();
        }



        public override void BeginMode()
        {
            PlayerController playerController = new SamplePlayerController();
        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);
        }
    }
}