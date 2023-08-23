using System;
using UnityEngine;

namespace TF.SettingMenu.Editor
{
    public static class EditorUtilities
    {
        public static void DrawLine(Color color, int height = 2)
        {
            var newStyle = new GUIStyle();
            newStyle.normal.background = CreateColorTexture(2, height, color);

            GUILayout.BeginVertical(newStyle);
            {
                GUILayout.Space(height);
            }
            GUILayout.EndVertical();
        }

        public static void DrawTitleWithIcon(string title, Color background, GUIStyle titleStyle, Texture2D icon, byte iconDistance = 0, Vector4? offset = null, params GUILayoutOption[] options)
        {
            var newStyle = new GUIStyle();
            newStyle.normal.background = CreateColorTexture(2, 2, background);

            var newOffset = offset != null ? offset.Value : Vector4.zero;

            string distance = string.Empty;

            for (int i = 0; i < iconDistance; i++)
            {
                distance += " ";
            }

            GUILayout.BeginVertical(newStyle);
            {
                GUILayout.Space(newOffset.x);

                GUILayout.BeginHorizontal(newStyle);
                {
                    GUILayout.Space(newOffset.z);

                    var content = new GUIContent
                    {
                        text = $"{distance}{title}",
                        image = icon,
                    };

                    GUILayout.Label(content, titleStyle, options);

                    GUILayout.Space(newOffset.w);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(newOffset.y);
            }
            GUILayout.EndVertical();
        }

        public static void DrawTitleWithIconAndButton(string title, Color background, GUIStyle titleStyle, Texture2D icon, Action onButton, Texture2D buttonIcon, Color buttonColor, byte iconDistance = 0, Vector4? offset = null, params GUILayoutOption[] options)
        {
            var newStyle = new GUIStyle();
            newStyle.normal.background = CreateColorTexture(2, 2, background);

            var newOffset = offset != null ? offset.Value : Vector4.zero;

            string distance = string.Empty;

            for (int i = 0; i < iconDistance; i++)
            {
                distance += " ";
            }

            GUILayout.BeginVertical(newStyle);
            {
                GUILayout.Space(newOffset.x);

                GUILayout.BeginHorizontal(newStyle);
                {
                    GUILayout.Space(newOffset.z);

                    var content = new GUIContent
                    {
                        text = $"{distance}{title}",
                        image = icon,
                    };

                    GUILayout.Label(content, titleStyle, options);

                    var buttonStyle = new GUIStyle("ToolbarButton");
                    buttonStyle.normal.background = CreateColorTexture(2, 2, buttonColor);
                    buttonStyle.alignment = TextAnchor.UpperRight;

                    if (GUILayout.Button(buttonIcon, buttonStyle, GUILayout.Width(28)))
                    {
                        onButton?.Invoke();
                    }

                    GUILayout.Space(newOffset.w);

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(newOffset.y);
            }
            GUILayout.EndVertical();
        }

        public static Texture2D CreateColorTexture(int width, int height, Color color)
        {
            Color32[] pix = new Color32[width * height];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = color;
            }

            var result = new Texture2D(width, height);
            result.SetPixels32(pix);
            result.Apply();

            return result;
        }
    }
}
