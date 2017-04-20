using UnityEngine;

namespace GameplayFramework.Sample
{
    public class GameplayFrameworkSampleMode : GameMode
    {
        protected override void SetGameState()
        {
            World.GameState = new GameStateSample();
        }



        public override void BeginMode()
        {
            PlayerController playerController = new PlayerControllerSample();
        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);
        }
    }
}