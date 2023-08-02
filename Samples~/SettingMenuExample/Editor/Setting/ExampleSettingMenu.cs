using TF.SettingMenu.Editor;
using UnityEditor;

namespace TF.Samples.SettingMenu.Editor
{
    public class ExampleSettingMenu : SettingComponentMenu<ExampleSetting>
    {
        private const string SETTING_NAME = "Example Setting";

        [MenuItem(DEFAULT_CREATE_MENU_PATH + SETTING_NAME, priority = 100)]
        public static void CreateSettingMenu()
        {
            CreateSettingMenu(SETTING_NAME);
        }
    }
}
