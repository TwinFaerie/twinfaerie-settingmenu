using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TF.SettingMenu.Editor
{
    internal static class SettingComponentValidator
    {
        private const string defaultPath = "Assets/Settings/";
        
        [MenuItem("TwinFaerie/Setting/Validate All Settings", priority = 2)]
        public static void ValidateAllSettingComponents()
        {
            GetAllSettingComponentTypes().ForEach(ValidateSettingComponent);
            AssetDatabase.Refresh();
        }
        
        private static List<Type> GetAllSettingComponentTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(SettingComponent)) && !t.IsAbstract)
                .ToList();
        }

        private static void ValidateSettingComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(SettingComponent))) return;

            var guidList = AssetDatabase.FindAssets($"t:{type.Name}")?.ToList();
            if (guidList is not null && guidList.Any())
            {
                var data = guidList.First();
                guidList.Remove(data);
                
                guidList.ForEach(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    AssetDatabase.DeleteAsset(path);
                    Debug.LogWarning($"Removed duplicate of {type.Name} asset at path :\n{path}");
                });
                
                return;
            }

            if (ScriptableObject.CreateInstance(type) is not SettingComponent item) return;
            
            var path = Path.Combine(defaultPath, item.DefaultPath);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            AssetDatabase.CreateAsset(item, path);
            AssetDatabase.SaveAssets();
            
            Debug.LogWarning($"Created {type.Name} at path :\n{path}, you can move it if necessary");
        }
    }
}