using System;
using System.IO;
using System.Linq;
using Boo.Lang;
using UnityEditor;
using UnityEngine;

namespace GameplayFramework.Internal
{
    public class EditorIntegration : EditorWindow
    {
        private static readonly string _relativeEnumDirectory = "GameplayFramework" + Path.DirectorySeparatorChar + "Enums";
        private static readonly string[] _sceneNamesToIgnore = new[] { "Start" };
        private static readonly string _assetDirectoryName = "Assets";

        private static readonly string _sceneNamesEnumRelativeFilePath = _relativeEnumDirectory + Path.DirectorySeparatorChar + "SceneName.cs";
        private static readonly string _sceneNamesEnumTemplate =
            "namespace GameplayFramework" + "\n" +
            "{{" + "\n\t" +
            "public enum SceneName" + "\n\t" +
            "{{" + "\n\t\t" +
            "{0}" + "\n\t" +
            "}}" + "\n" +
            "}}";

        private static readonly string _gameModeNamesEnumRelativeFilePath = _relativeEnumDirectory + Path.DirectorySeparatorChar + "GameModeName.cs";
        private static readonly string _gameModeNamesEnumTemplate =
            "namespace GameplayFramework" + "\n" +
            "{{" + "\n\t" +
            "public enum GameModeName" + "\n\t" +
            "{{" + "\n\t\t" +
            "{0}" + "\n\t" +
            "}}" + "\n" +
            "}}";



        private EditorIntegration()
        {
        }



        [MenuItem("GameplayFramework/Rebuild")]
        public static void RebuildAll()
        {
            // Validate asset directory.
            string assetDirectory = Application.dataPath;

            if(assetDirectory.EndsWith(_assetDirectoryName) == false)
                throw new InvalidOperationException("Maps can only be rebuilt from within the editor.");

            // Call all partial rebuilds.
            RebuildSceneEnum(assetDirectory);
            RebuildGameModeEnum(assetDirectory);

            Log("Rebuild complete.");
        }



        private static void RebuildSceneEnum(string assetDirectory)
        {
            // Get scenes that will be build.
            var scenes = EditorBuildSettings.scenes;

            if(scenes == null || scenes.Length == 0)
            {
                Debug.LogWarning("No scenes have been added to the build settings. The GameplayFramework cannot correctly rebuild.");
            }
            else
            {
                List<string> sceneNames = new List<string>();

                // Get all scenes to add to the enum.
                foreach(var scene in scenes)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scene.path);

                    if(_sceneNamesToIgnore.Contains(sceneName))
                        continue;

                    sceneNames.Add(sceneName);
                }

                string enumFileContent;

                // Build the contents of the enum file.
                if(sceneNames.Count == 0)
                {
                    enumFileContent = string.Format(_sceneNamesEnumTemplate, "");
                }
                else
                {
                    string enumMembers = string.Join(",\n\t\t", sceneNames.ToArray());
                    enumFileContent = string.Format(_sceneNamesEnumTemplate, enumMembers);
                }

                // Make sure the enum file exists.
                string fullEnumFilePath = Path.Combine(assetDirectory, _sceneNamesEnumRelativeFilePath);

                if(File.Exists(fullEnumFilePath) == false)
                    throw new InvalidOperationException("The following file could not be found: " + fullEnumFilePath);

                // Write contents.
                File.WriteAllText(fullEnumFilePath, enumFileContent);

                Log("If you are missing entries in the 'SceneName' enum, make sure to add the missing scenes to the build settings.");
            }
        }



        private static void RebuildGameModeEnum(string assetDirectory)
        {
            string gameModeDirectory = Path.Combine(assetDirectory, "GameModes");
            string enumFileContent;

            // Assume that there are no GameModes if the directory doesn't exist.
            if(Directory.Exists(gameModeDirectory) == false)
            {
                enumFileContent = string.Empty;
            }
            else
            {
                var filenames = new List<string>();

                // Get all files in the GameMode directory.
                foreach(var fullFilePath in Directory.GetFiles(gameModeDirectory))
                {
                    if(fullFilePath.EndsWith(".meta"))
                        continue;

                    filenames.Add(Path.GetFileNameWithoutExtension(fullFilePath));
                }

                // Build the contents of the enum file.
                if(filenames.Count == 0)
                {
                    enumFileContent = string.Format(_gameModeNamesEnumTemplate, "");
                }
                else
                {
                    string enumMembers = string.Join(",\n\t\t", filenames.ToArray());
                    enumFileContent = string.Format(_gameModeNamesEnumTemplate, enumMembers);
                }
            }

            // Make sure the enum file exists.
            string fullEnumFilePath = Path.Combine(assetDirectory, _gameModeNamesEnumRelativeFilePath);

            if(File.Exists(fullEnumFilePath) == false)
                throw new InvalidOperationException("The following file could not be found: " + fullEnumFilePath);

            // Write contents.
            File.WriteAllText(fullEnumFilePath, enumFileContent);

            Log("If you are missing entries in the 'GameModeName' enum, make sure to add the missing GameModes to the Assets/GameModes directory.");
        }



        private static void Log(string message)
        {
            Debug.Log("GameplayFramework: " + message);
        }
    }
}