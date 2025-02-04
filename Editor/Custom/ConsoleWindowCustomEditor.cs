using UnityEditor;
using UnityEngine;
using ConsoleWindowUtility = com.absence.consolesystem.internals.ConsoleWindowUtility;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// Custom editor for <see cref="ConsoleWindow"/>.
    /// </summary>
    [CustomEditor(typeof(ConsoleWindow), false)]
    public class ConsoleWindowCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ConsoleWindow window = (ConsoleWindow)target;
            bool showPreview = window.e_showPreview;

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;

            GUIStyle commandStyle = new GUIStyle(GUI.skin.box) { richText = true };

            EditorGUILayout.Space(20);

            showPreview = EditorGUILayout.Foldout(showPreview, "Commands Preview:", true, foldoutStyle);
            window.e_showPreview = showPreview;

            if (!showPreview) return;

            if (window.Profile == null)
            {
                EditorGUILayout.LabelField("There are no profiles selected. Select one to preview its commands.");
                return;
            }

            EditorGUI.indentLevel++;

            window.Profile.Commands.ForEach(command =>
            {
                GUIContent commandPreview = new GUIContent()
                {
                    text = ConsoleWindowUtility.GeneratePreviewForCommand(command, true),
                    tooltip = ConsoleWindowUtility.GenerateDetailedDescriptionForCommand(command),
                };

                GUILayout.Label(commandPreview, commandStyle);
            });

            EditorGUI.indentLevel--;

            ConsoleSystemEditorHelpers.DrawArgumentColorGuideGUILayout(10f);
        }
    }

}