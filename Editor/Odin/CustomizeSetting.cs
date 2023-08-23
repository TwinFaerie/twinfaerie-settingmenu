#if TF_HAS_TFODINEXTENDER
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TF.SettingMenu.Editor
{
    [CreateAssetMenu(fileName = "Customize Setting", menuName = "test", order = -500)]
    public class CustomizeSetting : ScriptableObject
    {
        public const string RESOURCES_PATH = "Assets/Resources/";
        public const string PATH = "TwinFaerie/SettingMenu/";
        public const string FILENAME = "Customize Setting.asset";

        public static CustomizeSetting GetInstance()
        {
            var result =  Resources.Load<CustomizeSetting>(Path.Combine(PATH, Path.GetFileNameWithoutExtension(FILENAME)));

            if (result == null)
            {
                Directory.CreateDirectory(RESOURCES_PATH + PATH);
                result = CreateInstance<CustomizeSetting>();
                AssetDatabase.CreateAsset(result, Path.Combine(RESOURCES_PATH, PATH, FILENAME));
            }

            return result;
        }

        [TabGroup("Data", Icon = SdfIconType.ClipboardData)] [HideLabel]
        [Title("Customize Setting Component", TitleAlignment = TitleAlignments.Centered)]
        [TableList(AlwaysExpanded = true, DrawScrollView = false, IsReadOnly = true, HideToolbar = true)]
        [SerializeField] private List<CustomizeData> customizeData = new();

        [TabGroup("Config", Icon = SdfIconType.GearFill)] [HideLabel]
        [SerializeField] private WindowOptions windowOptions = new();

        public WindowOptions WindowOptions => windowOptions;
        public List<CustomizeData> CustomizeData => customizeData;  

        public void RefreshData(List<SettingComponent> data)
        {
            var removedData = customizeData.Select(x => x.Name).Where(x => !data.Any(y => y.name == x));
            customizeData.RemoveAll(x => removedData.Contains(x.Name));

            var newData = data.Where(x => !customizeData.Any(y => x.name == y.Name));
            newData.ForEach(x => customizeData.Add(new(x)));
        }
    }

    public enum ToggleEnum 
    { 
        Enable, 
        Disable 
    }

    [Serializable]
    public class WindowOptions
    {
        #region Title

        [BoxGroup("Title")]
        public Font Font;

        [BoxGroup("Title")]
        public byte FontSize = 15;

        [BoxGroup("Title")]
        public FontStyle FontStyle = FontStyle.Bold;

        [BoxGroup("Title")]
        public TextAnchor Align = TextAnchor.MiddleLeft;

        [BoxGroup("Title")]
        public byte Height = 24;

        [BoxGroup("Title")]
        public Vector4 Offset = new Vector4(5, 5, 20, 10);

        [BoxGroup("Title")]
        public byte iconDistance = 1;

        #endregion Title

        #region Color

        [BoxGroup("Color")]
        public Color TitleColor = Color.white;

        [BoxGroup("Color")]
        public Color TitleBackgroundColor = Color.black;

        [BoxGroup("Color")]
        public Color TitleLineColor = Color.white;
        
        [BoxGroup("Color")]
        public Color TitleRefreshButtonColor = Color.black;

        [BoxGroup("Color")]
        public Color ItemTitleColor = Color.white;

        [BoxGroup("Color")]
        public Color ItemTitleBackgroundColor = Color.black;

        [BoxGroup("Color")]
        public Color ItemTitleLineColor = Color.white;

        #endregion Color
    }

    [Serializable]
    public class CustomizeData
    {
        [HideInInspector] private readonly SettingComponent data;

        [ReadOnly] [TableColumnWidth(200, Resizable = false)]
        [SerializeField] private string name;

        [PreviewField(Height = 20, Alignment = ObjectFieldAlignment.Center), TableColumnWidth(40, Resizable = false)]
        [SerializeField] private Sprite _icon;

        [TableColumnWidth(150, Resizable = false)]
        [SerializeField] private SdfIconType _iconDefault = SdfIconType.None;

        [SerializeField] private string _folderName = string.Empty;

        public SettingComponent Data => data;
        public string Name => name;
        public Sprite Icon => _icon;
        public SdfIconType IconDefault => _iconDefault;
        public string FolderName => _folderName;

        public CustomizeData(SettingComponent data)
        {
            this.data = data;
            name = data.name;
        }
    }
}
#endif