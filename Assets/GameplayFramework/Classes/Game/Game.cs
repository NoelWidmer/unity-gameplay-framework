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
            set
            {
                _current = value;

                TickWatch.Start();
                TickFixedWatch.Start();
            }
        }



        #region Time

        protected static readonly System.Diagnostics.Stopwatch TickWatch = new System.Diagnostics.Stopwatch();
        protected static readonly System.Diagnostics.Stopwatch TickFixedWatch = new System.Diagnostics.Stopwatch();

        public static float PlayTime
        {
            get;
            protected set;
        }

        #endregion

        #region Tick Events

        public static event TickHandler TickPlayerInput;
        public static event TickHandler TickControllers;
        public static event TickHandler TickActor;
        public static event TickHandler TickPlayerCamera;
        public static event TickHandler TickPlayerHUD;
        public static event TickHandler TickGameMode;
        
        public static event TickHandler TickFixed;



        public virtual void OnUnityUpdate()
        {
            float deltaTime = TickWatch.Elapsed.Milliseconds / 1000f;
            TickWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);
            PlayTime += deltaTime;

            RaiseTickInput(tickArgs);
            RaiseTickControl(tickArgs);
            RaiseTickActor(tickArgs);
            RaiseTickCamera(tickArgs);
            RaiseTickHUD(tickArgs);
            RaiseTickMode(tickArgs);

            Tick(tickArgs);
        }

        public virtual void OnUnityFixedUpdate()
        {
            float deltaTime = TickFixedWatch.Elapsed.Milliseconds / 1000f;
            TickFixedWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            RaiseTickFixed(tickArgs);
        }

        protected virtual void Tick(TickArgs e)
        {
            CheckLoadSceneCompletion();
        }

        #region Raise Events

        protected virtual void RaiseTickInput(TickArgs e)
        {
            var tickInput = TickPlayerInput;
            if(tickInput != null)
                tickInput(e);
        }

        protected virtual void RaiseTickControl(TickArgs e)
        {
            var tickControl = TickControllers;
            if(tickControl != null)
                tickControl(e);
        }

        protected virtual void RaiseTickActor(TickArgs e)
        {
            var tickActor = TickActor;
            if(tickActor != null)
                tickActor(e);
        }

        protected virtual void RaiseTickCamera(TickArgs e)
        {
            var tickCamera = TickPlayerCamera;
            if(tickCamera != null)
                tickCamera(e);
        }

        protected virtual void RaiseTickHUD(TickArgs e)
        {
            var tickHUD = TickPlayerHUD;
            if(tickHUD != null)
                tickHUD(e);
        }

        protected virtual void RaiseTickMode(TickArgs e)
        {
            var tickMode = TickGameMode;
            if(tickMode != null)
                tickMode(e);
        }



        protected virtual void RaiseTickFixed(TickArgs e)
        {
            var tickFixed = TickFixed;
            if(tickFixed != null)
                tickFixed(e);
        }

        #endregion

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
            string gameModeName = Enum.GetName(typeof(GameModeName), gameMode);

            Type type;

            // Get the type of game mode.
            {
                Type[] types = Assembly.GetExecutingAssembly().GetTypes();
                IEnumerable<Type> matchingTypes = types.Where(t => t.Name == gameModeName);

                if(matchingTypes.Count() == 1)
                    type = matchingTypes.First();
                else if(matchingTypes.Count() == 0)
                    throw new InvalidOperationException("Couldn't find a type with name '" + gameModeName + "'.");
                else
                    throw new InvalidOperationException("Found multiple types with name '" + gameModeName + "'.");
            }

            object instance;

            // Create an instance of the game mode.
            try
            {
                instance = Activator.CreateInstance(type);

                if(instance == null)
                    throw new InvalidOperationException("Couldn't create an instance of the type with name '" + gameModeName + "'.");
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Couldn't create an instance of the type with name '" + gameModeName + "'.", ex);
            }

            // Make sure the instance is a game mode.
            if(instance is GameMode)
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
            lock(_gameModeLock)
            {
                GameMode oldMode = GameMode;

                Debug.Log("Setting game mode: " + mode.GetType().Name);
                GameMode = mode;

                if(oldMode != null)
                    oldMode.Dispose();

                GameMode.BeginMode();
            }
        }



        public static GameState GameState
        {
            get;
            set;
        }

        #endregion

        #region Scene Loading

        public event EventHandler ScenePreLoad;
        public event EventHandler SceneLoadBegin;
        public event EventHandler ScenePostLoad;



        private readonly object _sceneLock = new object();
        private AsyncOperation _sceneLoader;



        public virtual void LoadScene(SceneName scene)
        {
            Debug.Log("Loading scene: " + scene.ToString());

            lock(_sceneLock)
            {
                if(_sceneLoader != null)
                    throw new InvalidOperationException("Only a single scene can be loaded at a time.");

                string sceneName = scene.ToString();

                var preLoadScene = ScenePreLoad;
                if(preLoadScene != null)
                    preLoadScene(null, EventArgs.Empty);

                _sceneLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                var duringLoadScene = SceneLoadBegin;
                if(duringLoadScene != null)
                    duringLoadScene(null, EventArgs.Empty);
            }
        }



        protected virtual void CheckLoadSceneCompletion()
        {
            // Check scene loading
            AsyncOperation sceneLoader = _sceneLoader;

            Debug.Log("Check scene loading process.");

            if(sceneLoader != null && sceneLoader.isDone)
            {
                lock(_sceneLock)
                {
                    _sceneLoader = null;

                    var postLoadScene = ScenePostLoad;
                    if(postLoadScene != null)
                        postLoadScene(null, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}