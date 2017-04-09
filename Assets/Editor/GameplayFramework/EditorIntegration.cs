using System;
using System.IO;
using Boo.Lang;
using UnityEditor;
using UnityEngine;

namespace GameplayFramework.Internal
{
    public class EditorIntegration : EditorWindow
    {
        private EditorIntegration()
        {
        }



        [MenuItem("GameplayFramework/RebuiltMaps")]
        public static void ShowWindow()
        {
            string dataPath = Application.dataPath;

            if(dataPath.EndsWith("Assets") == false)
                throw new InvalidOperationException("Maps can only be rebuilt from within the editor.");        

            string enumFileContent = "namespace GameplayFramework\n{\n\tpublic enum Map\n\t{\n\t\t*\n\t}\n}";

            {
                List<string> sceneNames = new List<string>();
                string sceneDirectory = Path.Combine(dataPath, "Scenes");

                foreach(var fullFilePath in Directory.GetFiles(sceneDirectory))
                {
                    if(fullFilePath.EndsWith("meta"))
                        continue;

                    sceneNames.Add(Path.GetFileNameWithoutExtension(fullFilePath));
                }

                string enumMembers = string.Join(",\n\t\t", sceneNames.ToArray());
                enumFileContent = enumFileContent.Replace("*", enumMembers);
            }

            string fullEnumFilePath = Path.Combine(dataPath, "GameplayFramework" + Path.DirectorySeparatorChar + "Map.cs");

            if(File.Exists(fullEnumFilePath) == false)
                throw new InvalidOperationException("The following file could not be found: " + fullEnumFilePath);

            File.WriteAllText(fullEnumFilePath, enumFileContent);
        }
    }
}