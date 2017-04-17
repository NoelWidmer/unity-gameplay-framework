using System;

namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        public PlayerController()
        {
            Initialize();
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



        protected virtual void Initialize()
        {
            PlayerInput = new PlayerInput();
            PlayerCamera = new PlayerCamera();
            PlayerHUD = new PlayerHUD();
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