using UnityEngine;

namespace GameplayFramework.Sample
{
    public class SamplePlayerController : PlayerController
    {
        public new SamplePlayerInput PlayerInput
        {
            get
            {
                return (SamplePlayerInput)base.PlayerInputManager;
            }
            set
            {
                base.PlayerInputManager = value;
            }
        }



        protected override void InitPlayerInputManager()
        {
            PlayerInput = new SamplePlayerInput();
        }



        protected override void BeginPlay()
        {
            base.BeginPlay();


        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);

            Debug.Log(PlayerInput.RightStick);
        }
    }
}