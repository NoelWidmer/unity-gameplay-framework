using UnityEngine;

namespace GameplayFramework.Sample
{
    public class PlayerControllerSample : PlayerController
    {
        public new PlayerInputSample PlayerInput
        {
            get
            {
                return (PlayerInputSample)base.PlayerInput;
            }
            set
            {
                base.PlayerInput = value;
            }
        }



        protected override void InitPlayerInput()
        {
            PlayerInput = new PlayerInputSample();
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