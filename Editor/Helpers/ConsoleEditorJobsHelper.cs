using com.absence.consolesystem.internals;
using UnityEditor;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// The static class responsible for managing the editor-side tasks of the console system.
    /// </summary>
    [InitializeOnLoad]
    internal static class ConsoleEditorJobsHelper
    {
        static ConsoleEditorJobsHelper()
        {
            ConsoleEventHandler.RefreshMethods(true);
        }

        [MenuItem("absencee_/absent-console/Refresh Methods")]
        static void RefreshMethods()
        {
            ConsoleEventHandler.RefreshMethods(true);
        }
    }

}