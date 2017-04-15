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
        #region Static

        private static readonly object _instanceLock = new object();
        private static Game _instance;      


        public static void Initialize(Game game)
        {
            if(game == null)
                throw new ArgumentNullException("game");

            lock(_instanceLock)
            {
                if(_instance != null)
                    throw new InvalidOperationException("The '" + typeof(Game).Name + "' can only be initialized once.");

                _instance = game;
            }

            PlayTime = Time.time;

            _normalWatch.Start();
            _lateWatch.Start();
            fixedWatch.Start();
        }



        public static event TickHandler TickInput;
        public static event TickHandler TickControl;
        public static event TickHandler TickCamera;
        public static event TickHandler TickHUD;
        public static event TickHandler TickActor;
        public static event TickHandler TickMode;

        public static event TickHandler TickLate;
        public static event TickHandler TickFixed;

        public static float PlayTime
        {
            get;
            protected set;
        }



        private static readonly System.Diagnostics.Stopwatch _normalWatch = new System.Diagnostics.Stopwatch();
        private static readonly System.Diagnostics.Stopwatch _lateWatch = new System.Diagnostics.Stopwatch();
        private static readonly System.Diagnostics.Stopwatch fixedWatch = new System.Diagnostics.Stopwatch();

        public static void OnUnityUpdate()
        {
            _instance.OnUnityUpdateImplementation();
        }

        public static void OnUnityLateUpdate()
        {
            _instance.OnUnityUpdateImplementation();
        }        

        public static void OnUnityFixedUpdate()
        {
            _instance.OnUnityFixedUpdateImplementation();
        }



        private static readonly object _gameModeLock = new object();

        public static GameMode GameMode
        {
            get;
            protected set;
        }

        public static void SetGameMode(GameModeName gameMode)
        {
            string gameModeName = Enum.GetName(typeof(GameModeName), gameMode);

            Type type;

            // Get the type of game mode.
            {
                Type[] types = Assembly.GetExecutingAssembly().GetTypes();
                IEnumerable<Type> matchingTypes = types.Where(t => t.Name == gameModeName);

                if(matchingTypes.Count() == 1)
                {
                    type = matchingTypes.First();
                }
                else if(matchingTypes.Count() == 0)
                {
                    throw new InvalidOperationException("Couldn't find a type with name '" + gameModeName + "'.");
                }
                else
                {
                    throw new InvalidOperationException("Found multiple types with name '" + gameModeName + "'.");
                }
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
            {
                _instance.SetGameMode((GameMode)instance);
            }
            else
            {
                throw new InvalidOperationException("The type with name '" + gameModeName + "' is not a '" + typeof(GameMode).Name + "'.");
            }
        }

        public static void SetGameMode<T>() where T : GameMode, new()
        {
            _instance.SetGameMode(new T());
        }



        public static GameState GameState
        {
            get;
            set;
        }


        
        private static readonly object _sceneLock = new object();
        private static AsyncOperation _sceneLoader;
        
        public static event EventHandler ScenePreLoad;
        public static event EventHandler SceneLoadBegin;
        public static event EventHandler ScenePostLoad;
        
        public static void LoadScene(SceneName scene)
        {
            _instance.LoadSceneImplementation(scene);
        }

        #endregion

        #region Instance

        protected virtual void OnUnityUpdateImplementation()
        {
            float deltaTime = _normalWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);
            PlayTime += deltaTime;

            {
                var tickInput = TickInput;
                if(tickInput != null)
                    tickInput(tickArgs);
            }

            {
                var tickControl = TickControl;
                if(tickControl != null)
                    tickControl(tickArgs);
            }

            {
                var tickCamera = TickCamera;
                if(tickCamera != null)
                    tickCamera(tickArgs);
            }

            {
                var tickHUD = TickHUD;
                if(tickHUD != null)
                    tickHUD(tickArgs);
            }

            {
                var tickActor = TickActor;
                if(tickActor != null)
                    tickActor(tickArgs);
            }

            {
                var tickMode = TickMode;
                if(tickMode != null)
                    tickMode(tickArgs);
            }

            Tick(tickArgs);
        }

        protected virtual void OnUnityLateUpdateImplementation()
        {
            float deltaTime = _lateWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            {
                var tickLate = TickLate;
                if(tickLate != null)
                    tickLate(tickArgs);
            }
        }

        protected virtual void OnUnityFixedUpdateImplementation()
        {
            float deltaTime = fixedWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            {
                var tickFixed = TickFixed;
                if(tickFixed != null)
                    tickFixed(tickArgs);
            }
        }

        protected virtual void SetGameMode(GameMode mode)
        {
            lock(_gameModeLock)
            {
                GameMode oldMode = GameMode;

                GameMode = mode;
                GameMode.Initialize();

                if(oldMode != null)
                    oldMode.EndMode();

                GameMode.BeginMode();
            }
        }

        protected virtual void LoadSceneImplementation(SceneName scene)
        {
            lock(_sceneLock)
            {
                if(_sceneLoader != null)
                    throw new InvalidOperationException("Only a single scene can be loaded at a time.");

                string sceneName = Enum.GetName(typeof(SceneName), scene);

                var preLoadScene = ScenePreLoad;
                if(preLoadScene != null)
                    preLoadScene(null, EventArgs.Empty);

                _sceneLoader = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                var duringLoadScene = SceneLoadBegin;
                if(duringLoadScene != null)
                    duringLoadScene(null, EventArgs.Empty);
            }
        }



        protected virtual void Tick(TickArgs e)
        {
            // Check scene loading
            AsyncOperation sceneLoader = _sceneLoader;

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