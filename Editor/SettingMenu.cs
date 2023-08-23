#if !TF_HAS_TFODINEXTENDER
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace TF.SettingMenu.Editor
{
    public class SettingMenu : EditorWindow
    {
        [SerializeField] private const string ICON_FILEPATH = "Packages/com.twinfaerie.settingmenu/Assets/Icon/Setting Icon.png";

        private VisualElement rightPanel;

        [MenuItem("TwinFaerie/Setting/Open Game Setting", priority = 1)]
        public static void ShowMenu()
        {
            EditorWindow window = GetWindow<SettingMenu>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ICON_FILEPATH);

            window.titleContent = new GUIContent("Game Setting", icon);
            window.minSize = new Vector2(1000, 600);
        }

        public void CreateGUI()
        {
            var allObjectGuids = AssetDatabase.FindAssets("t:SettingComponent");
            var allObjects = new List<SettingComponent>();

            foreach (var guid in allObjectGuids)
            {
                SettingComponent item = AssetDatabase.LoadAssetAtPath<SettingComponent>(AssetDatabase.GUIDToAssetPath(guid));

                if (item != null)
                {
                    allObjects.Add(item);
                }
            }

            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

            rootVisualElement.Add(splitView);

            var leftPanel = new ListView();
            splitView.Add(leftPanel);

            rightPanel = new ScrollView();
            splitView.Add(rightPanel);

            if (allObjects == null || allObjects.Count <= 0)
            { return; }

            leftPanel.makeItem = () => new VisualElement();
            leftPanel.bindItem = (item, index) => BindSettingMenuItem(item, allObjects[index].name, AssetDatabase.GUIDToAssetPath(allObjectGuids[index]));
            leftPanel.itemsSource = allObjects;

#if UNITY_2022_2_OR_NEWER
            leftPanel.selectionChanged += OnMenuSelectionChange;
#else
            leftPanel.onSelectionChange += OnMenuSelectionChange;
#endif
        }

        private void BindSettingMenuItem(VisualElement item, string name, string path)
        {
            FlexDirection flexDirection = FlexDirection.Row;
            item.style.flexDirection = flexDirection;

            var icon = new VisualElement();
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/Icon/{1}.png", Path.GetDirectoryName(path), name));
            var background = new StyleBackground(texture);
            icon.style.backgroundImage = background;
            icon.style.width = 20;
            icon.style.height = 20;
            item.Add(icon);

            var label = new Label();
            label.text = name;
            label.style.marginLeft = 5;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            item.Add(label);
        }

        private void OnMenuSelectionChange(IEnumerable<object> selectedItems)
        {
            rightPanel.Clear();

            var container = new VisualElement();

            container.style.paddingLeft = 5;
            container.style.paddingRight = 5;
            container.style.paddingTop = 5;
            container.style.paddingBottom = 5;

            var item = selectedItems.First() as SettingComponent;

            container.Add(CreateSettingTitle(item));
            container.Add(CreateUIElementInspector(item));
            rightPanel.Add(container);
        }

        private VisualElement CreateSettingTitle(SettingComponent setting)
        {
            var container = new VisualElement();

            var flexDirection = FlexDirection.Row;
            container.style.flexDirection = flexDirection;

            var styleColor = new StyleColor(Color.black);
            container.style.borderBottomColor = styleColor;
            container.style.borderBottomWidth = 2;
            container.style.paddingBottom = 5;
            container.style.marginBottom = 20;

            var icon = new VisualElement();
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(string.Format("{0}/Icon/{1}.png", Path.GetDirectoryName(AssetDatabase.GetAssetPath(setting)), Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(setting))));
            var background = new StyleBackground(texture);
            icon.style.backgroundImage = background;
            icon.style.width = 40;
            icon.style.height = 40;
            container.Add(icon);

            var title = new Label();
            title.text = setting.name;
            title.style.marginLeft = 10;
            title.style.unityTextAlign = TextAnchor.MiddleLeft;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 32;
            container.Add(title);

            return container;
        }

        private VisualElement CreateUIElementInspector(UnityEngine.Object target, List<string> propertiesToExclude = null)
        {
            var container = new VisualElement();

            var serializedObject = new SerializedObject(target);

            var fields = GetVisibleSerializedFields(target.GetType());

            for (int i = 0; i < fields.Length; ++i)
            {
                var field = fields[i];

                if (propertiesToExclude != null && propertiesToExclude.Contains(field.Name))
                {
                    continue;
                }

                var serializedProperty = serializedObject.FindProperty(field.Name);

                var propertyField = new PropertyField(serializedProperty);

                container.Add(propertyField);
            }

            container.Bind(serializedObject);


            return container;
        }

        private FieldInfo[] GetVisibleSerializedFields(Type T)
        {
            List<FieldInfo> infoFields = new List<FieldInfo>();

            // adding public non hide in inspector
            var publicFields = T.GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < publicFields.Length; i++)
            {
                if (publicFields[i].GetCustomAttribute<HideInInspector>() == null)
                {
                    infoFields.Add(publicFields[i]);
                }
            }

            // adding private serialized field
            var privateFields = T.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            for (int i = 0; i < privateFields.Length; i++)
            {
                if (privateFields[i].GetCustomAttribute<SerializeField>() != null)
                {
                    infoFields.Add(privateFields[i]);
                }
            }

            return infoFields.ToArray();
        }
    }
}
#endif