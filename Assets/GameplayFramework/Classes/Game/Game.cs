using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public class Game
    {
        private static Game _current;
        public static Game Current
        {
            get
            {
                return _current;
            }
        }



        public static void CreateNewGame(Game game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            game.TickWatch.Start();
            game.TickFixedWatch.Start();

            _current = game;
        }



        protected readonly System.Diagnostics.Stopwatch TickWatch = new System.Diagnostics.Stopwatch();
        protected readonly System.Diagnostics.Stopwatch TickFixedWatch = new System.Diagnostics.Stopwatch();

        public float PlayTime
        {
            get;
            protected set;
        }



        protected virtual void Tick(TickArgs e)
        {
            CheckLoadSceneCompletion();
        }



        #region Tick Events

        public virtual void OnUnityUpdate()
        {
            float deltaTime = TickWatch.Elapsed.Milliseconds / 1000f;
            TickWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);
            PlayTime += deltaTime;

            RaiseTickPlayerInputManagers(tickArgs);
            RaiseTickControllers(tickArgs);
            RaiseTickActors(tickArgs);
            RaiseTickPlayerCameraManagers(tickArgs);
            RaiseTickPlayerHUDManagers(tickArgs);
            RaiseTickGameMode(tickArgs);

            Tick(tickArgs);
        }

        public virtual void OnUnityFixedUpdate()
        {
            float fixedTime = TickFixedWatch.Elapsed.Milliseconds / 1000f;
            TickFixedWatch.Reset();

            TickArgs tickArgs = new TickArgs(fixedTime);

            RaiseTickFixed(tickArgs);
        }

        #endregion

        #region Game Mode & State

        private static readonly object _gameModeLock = new object();

        public static GameMode GameMode
        {
            get;
            protected set;
        }

        public void SetGameMode(GameModeName gameMode)
        {
            string gameModeName = gameMode.ToString();

            Type type;

            // Get the type of game mode.
            {
                List<Type> matchingTypes =
                    Assembly.GetExecutingAssembly().GetTypes().
                    Where(t => t.Name == gameModeName).
                    ToList();

                switch (matchingTypes.Count)
                {
                    case 0:
                        throw new InvalidOperationException("Couldn't find a type with name '" + gameModeName + "'.");
                    case 1:
                        type = matchingTypes[0];
                        break;
                    default:
                        throw new InvalidOperationException("Found multiple types with name '" + gameModeName + "'.");
                }
            }

            object instance;

            // Create an instance of the game mode.
            try
            {
                instance = Activator.CreateInstance(type);

                if (instance == null)
                    throw new InvalidOperationException("Couldn't create an instance of the type with name '" + gameModeName + "'.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Couldn't create an instance of the type with name '" + gameModeName + "'.", ex);
            }

            // Make sure the instance is a game mode.
            if (instance is GameMode)
                _current.SetGameMode((GameMode)instance);
            else
                throw new InvalidOperationException("The type with name '" + gameModeName + "' is not a '" + typeof(GameMode).Name + "'.");
        }

        public virtual void SetGameMode<T>() where T : GameMode, new()
        {
            var mode = new T();
            SetGameMode(mode);
        }

        protected virtual void SetGameMode(GameMode mode)
        {
            lock (_gameModeLock)
            {
                GameMode oldMode = GameMode;

                if (oldMode != null)
                    oldMode.BeforeEndMode();

                Debug.Log("Setting game mode: " + mode.GetType().Name);
                GameMode = mode;
                mode.BeginMode();

                if (oldMode != null)
                    oldMode.EndMode();
            }
        }



        public GameState GameState
        {
            get;
            set;
        }

        #endregion

        #region Scene Loading

        private readonly object _sceneLock = new object();
        private AsyncOperation _sceneLoader;



        public virtual void LoadScene(SceneName scene)
        {
            Debug.Log("Loading scene: " + scene.ToString());

            lock (_sceneLock)
            {
                if (_sceneLoader != null)
                    throw new InvalidOperationException("Only a single scene can be loaded at a time.");

                string sceneName = scene.ToString();

                RaiseScenePreLoad(EventArgs.Empty);
                _sceneLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
        }



        protected virtual void CheckLoadSceneCompletion()
        {
            // Check scene loading
            AsyncOperation sceneLoader = _sceneLoader;

            Debug.Log("Check scene loading process.");

            if (sceneLoader != null && sceneLoader.isDone)
            {
                lock (_sceneLock)
                {
                    _sceneLoader = null;
                    RaiseScenePostLoad(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Raise events

        public static event TickHandler TickPlayerInputManagers;
        protected virtual void RaiseTickPlayerInputManagers(TickArgs e)
        {
            var tickInput = TickPlayerInputManagers;
            if (tickInput != null)
                tickInput(e);
        }

        public static event TickHandler TickControllers;
        protected virtual void RaiseTickControllers(TickArgs e)
        {
            var tickControl = TickControllers;
            if (tickControl != null)
                tickControl(e);
        }

        public static event TickHandler TickActors;
        protected virtual void RaiseTickActors(TickArgs e)
        {
            var tickActor = TickActors;
            if (tickActor != null)
                tickActor(e);
        }

        public static event TickHandler TickPlayerCameraManagers;
        protected virtual void RaiseTickPlayerCameraManagers(TickArgs e)
        {
            var tickCamera = TickPlayerCameraManagers;
            if (tickCamera != null)
                tickCamera(e);
        }

        public static event TickHandler TickPlayerHUDManagers;
        protected virtual void RaiseTickPlayerHUDManagers(TickArgs e)
        {
            var tickHUD = TickPlayerHUDManagers;
            if (tickHUD != null)
                tickHUD(e);
        }

        public static event TickHandler TickGameMode;
        protected virtual void RaiseTickGameMode(TickArgs e)
        {
            var tickMode = TickGameMode;
            if (tickMode != null)
                tickMode(e);
        }

        public static event TickHandler TickFixed;
        protected virtual void RaiseTickFixed(TickArgs e)
        {
            var tickFixed = TickFixed;
            if (tickFixed != null)
                tickFixed(e);
        }



        public event EventHandler ScenePreLoad;
        protected void RaiseScenePreLoad(EventArgs e)
        {
            var scenePreLoad = ScenePreLoad;
            if (scenePreLoad != null)
                scenePreLoad(Game.Current, e);
        }

        public event EventHandler ScenePostLoad;
        protected void RaiseScenePostLoad(EventArgs e)
        {
            var scenePostLoad = ScenePostLoad;
            if (scenePostLoad != null)
                scenePostLoad(Game.Current, e);
        }

        #endregion
    }
}