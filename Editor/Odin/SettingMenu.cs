#if TF_HAS_TFODINEXTENDER
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using TF.SettingMenu.Editor;
using UnityEditor;
using UnityEngine;

namespace TF.SettingMenu.Odin.Editor
{
    public class SettingMenu : OdinMenuEditorWindow
    {
        private readonly List<SettingComponent> settingList = new();
        private CustomizeSetting customize;

        [MenuItem("TwinFaerie/Setting/Open Game Setting", priority = 1)]
        public static void ShowMenu()
        {
            EditorWindow window = GetWindow<SettingMenu>();

            window.titleContent = new GUIContent("Game Setting", EditorIcons.SettingsCog.Raw);
            window.minSize = new Vector2(1000, 600);
        }

        protected override void Initialize()
        {
            InitializeCustomizeIfNeeded();
            RefreshData();
        }

        private void InitializeCustomizeIfNeeded()
        {
            if (customize != null)
            { return; }

            customize = CustomizeSetting.GetInstance();
        }

        private void RefreshData()
        {
            settingList.Clear();

            var allObjectGuids = AssetDatabase.FindAssets("t:SettingComponent");
            foreach (var guid in allObjectGuids)
            {
                var item = AssetDatabase.LoadAssetAtPath<SettingComponent>(AssetDatabase.GUIDToAssetPath(guid));

                if (item is not null)
                {
                    settingList.Add(item);
                }
            }

            customize.RefreshData(settingList);
        }

        private void DrawTitle(string name, Texture2D icon = null, Color? overrideTextColor = null, Color? overrideBackgroundColor = null)
        {
            var style = new GUIStyle
            {
                font = customize.WindowOptions.Font,
                fontStyle = customize.WindowOptions.FontStyle,
                fontSize = customize.WindowOptions.FontSize,
                alignment = customize.WindowOptions.Align
            };

            style.normal.textColor = overrideTextColor is null ? customize.WindowOptions.TitleColor : overrideTextColor.Value;

            EditorUtilities.DrawTitleWithIcon(
                name,
                overrideBackgroundColor is null ? customize.WindowOptions.TitleBackgroundColor : overrideBackgroundColor.Value,
                style,
                icon,
                customize.WindowOptions.iconDistance,
                customize.WindowOptions.Offset,
                GUILayout.Height(customize.WindowOptions.Height)
            );

            EditorUtilities.DrawLine(customize.WindowOptions.ItemTitleLineColor);
        }

        private void DrawTitleAndRefresh(string name, Texture2D icon = null, Color? overrideTextColor = null, Color? overrideBackgroundColor = null)
        {
            var style = new GUIStyle
            {
                font = customize.WindowOptions.Font,
                fontStyle = customize.WindowOptions.FontStyle,
                fontSize = customize.WindowOptions.FontSize,
                alignment = customize.WindowOptions.Align
            };

            style.normal.textColor = overrideTextColor is null ? customize.WindowOptions.TitleColor : overrideTextColor.Value;

            EditorUtilities.DrawTitleWithIconAndButton(
                name,
                overrideBackgroundColor is null ? customize.WindowOptions.TitleBackgroundColor : overrideBackgroundColor.Value,
                style,
                icon,
                RefreshData,
                EditorIcons.Refresh.Raw,
                customize.WindowOptions.TitleRefreshButtonColor,
                customize.WindowOptions.iconDistance,
                customize.WindowOptions.Offset,
                GUILayout.Height(customize.WindowOptions.Height)
            );

            EditorUtilities.DrawLine(customize.WindowOptions.ItemTitleLineColor);
        }

        private void DrawCustomizeSetting(ref OdinMenuTree menuTree)
        {
            menuTree.Add("Customize Menu", customize, EditorIcons.SettingsCog);
        }

        private void DrawAllSetting(ref OdinMenuTree menuTree)
        {
            if (settingList == null)
            { return; }

            foreach (var item in settingList)
            {
                var customizeData = customize.CustomizeData.Find(x => x.Name == item.name);
                if (customizeData is null)
                { return; };    

                var path = $"{customizeData.FolderName}/{item.name}";

                if (customizeData.Icon == null && customizeData.IconDefault != SdfIconType.None)
                {
                    menuTree.Add(path, item, customizeData.IconDefault);
                }
                else
                {
                    menuTree.Add(path, item, customizeData.Icon);
                }
            }
        }

        protected void SaveAndRefreshWindow()
        {
            ForceMenuTreeRebuild();

            Undo.RecordObject(customize, "Setting Menu Customize");
            AssetDatabase.Refresh();
        }

        protected override void OnImGUI()
        {
            wantsMouseEnterLeaveWindow = true;

            DrawTitleAndRefresh("Game Setting", EditorIcons.SettingsCog.Raw);

            EditorGUI.BeginChangeCheck();
            {
                base.OnImGUI();
            }
            if (EditorGUI.EndChangeCheck() || Event.current.type == EventType.MouseEnterWindow || Event.current.type == EventType.MouseLeaveWindow)
            {
                SaveAndRefreshWindow();
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            InitializeCustomizeIfNeeded();

            var menuTree = new OdinMenuTree();

            DrawCustomizeSetting(ref menuTree);
            DrawAllSetting(ref menuTree);

            return menuTree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree.Selection.SelectedValue is null)
            { return; }

            if (MenuTree.Selection.SelectedValue is SettingComponent item)
            {
                //var icon = customize.CustomizeData[item].Icon == null ? customize.CustomizeData[item].IconDefault  : customize.CustomizeData[item].Icon.texture

                DrawTitle(item.name, null, customize.WindowOptions.ItemTitleColor, customize.WindowOptions.ItemTitleBackgroundColor);
                return;
            }
        }
    }
}
#endif