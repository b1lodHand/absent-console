using com.absence.consolesystem.internals;
using UnityEditor;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// The static class responsible for managing the editor-side tasks of the console system.
    /// </summary>
    [InitializeOnLoad]
    internal static class EditorJobsHelper
    {
        static EditorJobsHelper()
        {
            ConsoleEventDatabase.RefreshMethods(ConsoleEventDatabase.DEBUG_MODE);
        }

        [MenuItem("absencee_/absent-console/Refresh Method Database")]
        static void RefreshMethods()
        {
            ConsoleEventDatabase.RefreshMethods(true);
        }
    }

}