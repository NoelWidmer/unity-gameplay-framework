using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameplayFramework
{
    public static class Game
    {
        private static readonly System.Diagnostics.Stopwatch _normalWatch = new System.Diagnostics.Stopwatch();
        private static readonly System.Diagnostics.Stopwatch _lateWatch = new System.Diagnostics.Stopwatch();
        private static readonly System.Diagnostics.Stopwatch _fixedWatch = new System.Diagnostics.Stopwatch();
        


        public static void Initialize()
        {
            _normalWatch.Start();
            _lateWatch.Start();
            _fixedWatch.Start();
        }



        #region Tick Events

        public static event TickHandler TickInput;
        public static event TickHandler TickControl;
        public static event TickHandler TickCamera;
        public static event TickHandler TickHUD;
        public static event TickHandler TickActor;
        public static event TickHandler TickMode;

        public static event TickHandler TickLate;
        public static event TickHandler TickFixed;

        private static float _gameTime;



        public static void OnUnityUpdate()
        {
            float deltaTime = _normalWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);
            _gameTime += deltaTime;

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



        public static void OnUnityLateUpdate()
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



        public static void OnUnityFixedUpdate()
        {
            float deltaTime = _fixedWatch.Elapsed.Milliseconds / 1000f;
            _normalWatch.Reset();

            TickArgs tickArgs = new TickArgs(deltaTime);

            {
                var tickFixed = TickFixed;
                if(tickFixed != null)
                    tickFixed(tickArgs);
            }
        }

        #endregion

        #region GameMode

        private static readonly object _gameModeLock = new object();
        private static GameMode _gameMode;

        public static GameMode GameMode
        {
            get
            {
                return _gameMode;
            }
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
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Couldn't create an instance of the type with name '" + gameModeName + "'.", ex);
            }

            // Make sure the instance is a game mode.
            if(instance is GameMode)
            {
                SetGameMode((GameMode)instance);
            }
            else
            {
                throw new InvalidOperationException("The type with name '" + gameModeName + "' is not a '" + typeof(GameMode).Name + "'.");
            }
        }



        public static void SetGameMode<T>() where T : GameMode, new()
        {
            SetGameMode(new T());
        }



        private static void SetGameMode(GameMode mode)
        {
            lock(_gameModeLock)
            {
                GameMode oldMode = _gameMode;

                _gameMode = mode;
                _gameMode.Initialize();

                if(oldMode != null)
                    oldMode.EndMode();

                _gameMode.BeginMode();
            }
        }

        #endregion

        #region Game State

        private static GameState _gameState;
        public static GameState GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
            }
        }

        #endregion

        #region Scene loading

        private static readonly object _sceneLock = new object();
        private static AsyncOperation _sceneLoader;


        public static event EventHandler ScenePreLoad;
        public static event EventHandler SceneLoadBegin;
        public static event EventHandler ScenePostLoad;



        public static void LoadScene(SceneName scene)
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

        private static void Tick(TickArgs e)
        {
            // Check scene loading
            {
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
        }

        #endregion
    }
}