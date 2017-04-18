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



        protected override void InitializePlayerComponents()
        {
            PlayerInput = new PlayerInputSample();
            PlayerCamera = new PlayerCamera();
            PlayerHUD = new PlayerHUD();
        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);

            Debug.Log(PlayerInput.LeftStick);
        }
    }
}