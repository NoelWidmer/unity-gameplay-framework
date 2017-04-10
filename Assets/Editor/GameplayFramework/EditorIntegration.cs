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
        private static readonly string[] _buildSceneNamesToIgnore = new [] { "Start" };
        private static readonly string _assetDirectoryName = "Assets";
        private static readonly string _mapEnumTemplate = 
            "namespace GameplayFramework" + "\n" + 
            "{{" + "\n\t" + 
            "public enum Map" + "\n\t" + 
            "{{" + "\n\t\t" + 
            "{0}" + "\n\t" + 
            "}}" + "\n" + 
            "}}";



        private EditorIntegration()
        {
        }



        [MenuItem("GameplayFramework/Rebuild")]
        public static void ShowWindow()
        {
            // Validate asset directory.
            string dataPath = Application.dataPath;

            if(dataPath.EndsWith(_assetDirectoryName) == false)
                throw new InvalidOperationException("Maps can only be rebuilt from within the editor.");

            // Get scenes that will be build.
            var buildScenes = EditorBuildSettings.scenes;

            if(buildScenes == null || buildScenes.Length == 0)
            {
                Debug.LogWarning("No scenes have been added to the build settings. The GameplayFramework cannot correctly rebuild.");
            }
            else
            {
                List<string> buildSceneNames = new List<string>();

                // Get all scenes to add to the enum.
                foreach(var buildScene in buildScenes)
                {
                    string buildSceneName = Path.GetFileNameWithoutExtension(buildScene.path);

                    if(_buildSceneNamesToIgnore.Contains(buildSceneName))
                        continue;

                    buildSceneNames.Add(buildSceneName);
                }

                string enumFileContent;

                // Build the contents of the enum file.
                if(buildSceneNames.Count == 0)
                {
                    enumFileContent = string.Format(_mapEnumTemplate, "");
                }
                else
                {
                    string enumMembers = string.Join(",\n\t\t", buildSceneNames.ToArray());
                    enumFileContent = string.Format(_mapEnumTemplate, enumMembers);
                }

                // Get the path of the enum file.
                string fullEnumFilePath = Path.Combine(dataPath, "GameplayFramework" + Path.DirectorySeparatorChar + "Map.cs");

                if(File.Exists(fullEnumFilePath) == false)
                    throw new InvalidOperationException("The following file could not be found: " + fullEnumFilePath);

                // Write contents.
                File.WriteAllText(fullEnumFilePath, enumFileContent);

                Debug.Log("The GameplayFramework has rebuilt. If you are missing entries in the Maps enum, make sure to add those maps to the build settings.");
            }
        }
    }
}