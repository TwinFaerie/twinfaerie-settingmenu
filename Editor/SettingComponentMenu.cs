using System.IO;
using UnityEditor;
using UnityEngine;

namespace TF.SettingMenu.Editor
{
    public abstract class SettingComponentMenu<T> where T : SettingComponent
    {
        private const string DEFAULT_PATH = "Assets/Setting/";
        protected const string DEFAULT_CREATE_MENU_PATH = "TwinFaerie/Setting/Setup/Create ";

        // use this command on child class
        //[MenuItem(DEFAULT_CREATE_MENU_PATH + "{YOUR_SETTING_NAME}", priority = 10)]

        // call this function and specify the name when using the command above
        protected static void CreateSettingMenu(string name)
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (!Directory.Exists(DEFAULT_PATH))
            {
                Directory.CreateDirectory(DEFAULT_PATH);
            }

            var filePath = string.Format("{0}{1}.asset", DEFAULT_PATH, name);

            if (!File.Exists(filePath))
            {
                AssetDatabase.CreateAsset(asset, string.Format("{0}{1}.asset", DEFAULT_PATH, name));
                AssetDatabase.SaveAssets();

                EditorUtility.DisplayDialog("Setting creation Success", "Setting creation Success, now you can access it in Game Setting Menu", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Setting creation Failed", "Setting already created, no need to create it twice!", "OK");
            }
        }
    }
}