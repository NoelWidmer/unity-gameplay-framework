using System;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        public PlayerController()
        {
            InitPlayerInput();
            InitPlayerCamera();
            InitPlayerHUD();
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



        protected virtual void InitPlayerInput()
        {
            PlayerInput = new PlayerInput();
        }

        protected virtual void InitPlayerCamera()
        {
            PlayerCamera = new PlayerCamera();
        }

        protected virtual void InitPlayerHUD()
        {
            PlayerHUD = new PlayerHUD();
        }



        protected virtual void BeginPlay()
        {
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