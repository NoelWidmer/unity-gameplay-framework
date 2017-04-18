using System;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        public PlayerController()
        {
            InitializePlayerComponents();
        }


        
        public PlayerInput PlayerInput
        {
            get;
            protected set;
        }
        
        public PlayerCamera PlayerCamera
        {
            get;
            protected set;
        }
        
        public PlayerHUD PlayerHUD
        {
            get;
            protected set;
        }



        protected virtual void InitializePlayerComponents()
        {
            PlayerInput = new PlayerInput();
            PlayerCamera = new PlayerCamera();
            PlayerHUD = new PlayerHUD();
        }



        protected override void Tick(TickArgs e)
        {
            base.Tick(e);

            PlayerInput playerInput = PlayerInput;
            if(playerInput != null)
                playerInput.Reset();
        }



        public override void Dispose()
        {
            base.Dispose();
            
            if(PlayerInput != null)
            {
                PlayerInput.Dispose();
                PlayerInput = null;
            }

            if(PlayerCamera != null)
            {
                PlayerCamera.Dispose();
                PlayerCamera = null;
            }

            if(PlayerHUD != null)
            {
                PlayerHUD.Dispose();
                PlayerHUD = null;
            }
        }
    }
}