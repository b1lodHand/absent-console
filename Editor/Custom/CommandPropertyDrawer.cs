using com.absence.consolesystem.internals;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// Custom property drawer for <see cref="Command"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(Command))]
    public class CommandPropertyDrawer : PropertyDrawer
    {
        const float k_verticalSpacing = 5f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty argsProp = property.FindPropertyRelative("m_arguments");
            bool isExpanded = property.FindPropertyRelative("m_isExpanded").boolValue;

            if (isExpanded) return (EditorGUIUtility.singleLineHeight * 4) + EditorGUI.GetPropertyHeight(argsProp, true) + (k_verticalSpacing * 4);
            else return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Command command = (Command)property.boxedValue;

            SerializedProperty isExpandedProp = property.FindPropertyRelative("m_isExpanded");
            SerializedProperty keywordProp = property.FindPropertyRelative("m_keyword");
            SerializedProperty descriptionProp = property.FindPropertyRelative("m_description");
            SerializedProperty methodPreviewProp = property.FindPropertyRelative("m_methodPreview");
            SerializedProperty argsProp = property.FindPropertyRelative("m_arguments");

            float totalHeight = position.height;
            position.height = EditorGUIUtility.singleLineHeight;

            GUIStyle foldoutLabelStyle = new GUIStyle(EditorStyles.foldout);
            foldoutLabelStyle.richText = true;

            bool isExpanded = isExpandedProp.boolValue;
            string preview = ConsoleWindowUtility.GeneratePreviewForCommand(command, true);
            GUIContent labelContent = new GUIContent()
            {
                text = preview,
                tooltip = ConsoleWindowUtility.GenerateDetailedDescriptionForCommand(command),
            };

            isExpanded = EditorGUI.Foldout(position, isExpanded, labelContent, true, foldoutLabelStyle);
            isExpandedProp.boolValue = isExpanded;

            if (!isExpanded) return;

            position.y += EditorGUIUtility.singleLineHeight;

            if (Application.isPlaying) GUI.enabled = false;

            string keyword = keywordProp.stringValue;
            keyword = EditorGUI.TextField(position, "Keyword: ", keyword);
            keywordProp.stringValue = keyword;

            position.y += EditorGUIUtility.singleLineHeight;
            position.y += k_verticalSpacing;

            string description = descriptionProp.stringValue;
            description = EditorGUI.TextField(position, "Description: ", description);
            descriptionProp.stringValue = description;

            position.y += EditorGUIUtility.singleLineHeight;
            position.y += k_verticalSpacing;

            DrawMethodSelector();

            position.y += EditorGUIUtility.singleLineHeight;
            position.y += k_verticalSpacing;

            position.height = totalHeight - (2 * EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(position, argsProp, new GUIContent() { text = "Arguments " }, true);

            GUI.enabled = true;

            return;

            void DrawMethodSelector()
            {
                List<MethodInfo> suitableMethods = ConsoleEventHandler.GetSuitableMethodsForCommand(command);

                if (suitableMethods.Count == 0)
                {
                    EditorGUI.Popup(position, "Method to Invoke: ", 0, new string[] { "There are no suitable methods for this command. " });
                    methodPreviewProp.stringValue = "";
                    return;
                }

                List<string> suitablePreviews = suitableMethods.ConvertAll(method => ConsoleEventHandler.GenerateMethodPreview(method));

                string selectedMethodPreview = methodPreviewProp.stringValue;
                int methodIndex = 0;

                if (suitablePreviews.Contains(selectedMethodPreview)) methodIndex = suitablePreviews.IndexOf(selectedMethodPreview);

                methodIndex = EditorGUI.Popup(position, "Method to Invoke: ", methodIndex, suitablePreviews.ToArray());
                selectedMethodPreview = suitablePreviews[methodIndex];

                methodPreviewProp.stringValue = selectedMethodPreview;

                if (!suitablePreviews.Contains(selectedMethodPreview)) selectedMethodPreview = "";
            }
        }
    }

}