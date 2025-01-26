using UnityEditor;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// Custom editor for <see cref="ConsoleProfile"/>.
    /// </summary>
    [CustomEditor(typeof(ConsoleProfile), false)]
    public class ConsoleProfileCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ConsoleSystemEditorHelpers.DrawArgumentColorGuideGUILayout(10f);
        }
    }
}
