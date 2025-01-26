using com.absence.consolesystem.internals;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ConsoleWindowUtility = com.absence.consolesystem.internals.ConsoleWindowUtility;

namespace com.absence.consolesystem.editor
{
    public static class ConsoleSystemEditorHelpers
    {
        const float k_boxSize = 10f;
        const float k_boxOutlineThickness = 2f;

        static Dictionary<Argument.ArgumentValueType, Color> s_colorPairs;
        static Color s_boxOutlineColor = Color.black;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            s_colorPairs = new();

            Dictionary<Argument.ArgumentValueType, string> m_dictionary = ConsoleWindowUtility.ColorTags;
            foreach (KeyValuePair<Argument.ArgumentValueType, string> kvp in m_dictionary)
            {
                if (ColorUtility.TryParseHtmlString(kvp.Value, out Color parsedColor))
                    s_colorPairs.Add(kvp.Key, parsedColor);
                else
                    s_colorPairs.Add(kvp.Key, Color.grey);
            }
        }

        public static float DrawArgumentColorGuideGUI(Rect startRect, float verticalSpace)
        {
            Dictionary<Argument.ArgumentValueType, string> m_dictionary = ConsoleWindowUtility.ColorTags;
            float totalHeight = m_dictionary.Count * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight;

            GUIStyle style = new(GUI.skin.label);

            startRect = EditorGUI.IndentedRect(startRect);
            startRect.y += verticalSpace;
            startRect.height = height;

            Rect dynamicBoxRect = Rect.zero;
            Rect boxBackroundRect = Rect.zero;

            bool first = true;
            foreach (KeyValuePair<Argument.ArgumentValueType, Color> kvp in s_colorPairs)
            {
                GUIContent labelContent = new(kvp.Key.ToString());

                if (first)
                {
                    float sizeOffset = (EditorGUIUtility.singleLineHeight - k_boxSize) / 2;

                    dynamicBoxRect = startRect;
                    dynamicBoxRect.x -= k_boxSize;
                    dynamicBoxRect.x -= spacing;
                    dynamicBoxRect.y += sizeOffset;
                    dynamicBoxRect.width = k_boxSize;
                    dynamicBoxRect.height = k_boxSize;

                    if (k_boxOutlineThickness != 0f)
                    {
                        float thicknessModifier = k_boxOutlineThickness / 2f;

                        boxBackroundRect = dynamicBoxRect;
                        boxBackroundRect.width += k_boxOutlineThickness;
                        boxBackroundRect.height += k_boxOutlineThickness;
                        boxBackroundRect.x -= thicknessModifier;
                        boxBackroundRect.y -= thicknessModifier;
                    }

                    first = false;
                }

                Vector2 labelSizeRaw = style.CalcSize(labelContent);
                Vector2 labelSize = style.CalcScreenSize(labelSizeRaw);

                startRect.width = labelSize.x;
                EditorGUI.LabelField(startRect, labelContent);

                if (k_boxOutlineThickness != 0f) EditorGUI.DrawRect(boxBackroundRect, s_boxOutlineColor);
                EditorGUI.DrawRect(dynamicBoxRect, kvp.Value);

                dynamicBoxRect.y += height;
                dynamicBoxRect.y += spacing;
                boxBackroundRect.y += height;
                boxBackroundRect.y += spacing;
                startRect.y += height;
                startRect.y += spacing;
            }

            return totalHeight;
        }

        public static void DrawArgumentColorGuideGUILayout(float verticalSpace)
        {
            EditorGUILayout.Space(verticalSpace);

            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight;

            EditorGUI.indentLevel++;
            Rect dynamicBoxRect = Rect.zero;
            Rect boxBackroundRect = Rect.zero;

            bool first = true;
            foreach (KeyValuePair<Argument.ArgumentValueType, Color> kvp in s_colorPairs) 
            {
                EditorGUILayout.LabelField(kvp.Key.ToString());

                if (first)
                {
                    float sizeOffset = (EditorGUIUtility.singleLineHeight - k_boxSize) / 2;

                    dynamicBoxRect = EditorGUI.IndentedRect(GUILayoutUtility.GetLastRect());
                    dynamicBoxRect.x -= k_boxSize;
                    dynamicBoxRect.x -= spacing;
                    dynamicBoxRect.y += sizeOffset;
                    dynamicBoxRect.width = k_boxSize;
                    dynamicBoxRect.height = k_boxSize;

                    if (k_boxOutlineThickness != 0f)
                    {
                        float thicknessModifier = k_boxOutlineThickness / 2f;

                        boxBackroundRect = dynamicBoxRect;
                        boxBackroundRect.width += k_boxOutlineThickness;
                        boxBackroundRect.height += k_boxOutlineThickness;
                        boxBackroundRect.x -= thicknessModifier;
                        boxBackroundRect.y -= thicknessModifier;
                    }

                    first = false;
                }

                if (k_boxOutlineThickness != 0f) EditorGUI.DrawRect(boxBackroundRect, s_boxOutlineColor);
                EditorGUI.DrawRect(dynamicBoxRect, kvp.Value);

                dynamicBoxRect.y += height;
                dynamicBoxRect.y += spacing;
                boxBackroundRect.y += height;
                boxBackroundRect.y += spacing;
            }

            EditorGUI.indentLevel--;
        }
    }
}
