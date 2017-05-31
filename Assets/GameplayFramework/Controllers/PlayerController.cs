namespace GameplayFramework
{
    public class PlayerController : Controller
    {
        public PlayerController()
        {
            InitPlayerInputManager();
            InitPlayerCameraManager();
            InitPlayerHUDManager();
        }


        
        public PlayerInputManager PlayerInputManager
        {
            get;
            protected set;
        }
        
        public PlayerCameraManager PlayerCameraManager
        {
            get;
            protected set;
        }
        
        public PlayerHUDManager PlayerHUDManager
        {
            get;
            protected set;
        }



        protected virtual void InitPlayerInputManager()
        {
            PlayerInputManager = new PlayerInputManager();
            PlayerInputManager.TickEnabled = true;
        }

        protected virtual void InitPlayerCameraManager()
        {
            PlayerCameraManager = new PlayerCameraManager();
            PlayerCameraManager.TickEnabled = true;
        }

        protected virtual void InitPlayerHUDManager()
        {
            PlayerHUDManager = new PlayerHUDManager();
            PlayerHUDManager.TickEnabled = true;
        }



        protected virtual void BeginPlay()
        {
        }



        protected override void Tick(TickArgs e)
        {
        }



        public override void Dispose()
        {
            base.Dispose();

            if(PlayerInputManager != null)
            {
                PlayerInputManager.Dispose();
                PlayerInputManager = null;
            }
            
            if(PlayerCameraManager != null)
            {
                PlayerCameraManager.Dispose();
                PlayerCameraManager = null;
            }

            if(PlayerHUDManager != null)
            {
                PlayerHUDManager.Dispose();
                PlayerHUDManager = null;
            }
        }
    }
}