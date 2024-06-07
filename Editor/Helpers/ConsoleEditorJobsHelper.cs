using com.absence.consolesystem.internals;
using UnityEditor;

namespace com.absence.consolesystem.editor
{
    [InitializeOnLoad]
    internal static class ConsoleEditorJobsHelper
    {
        static ConsoleEditorJobsHelper()
        {
            ConsoleEventDatabase.RefreshMethods(true);
        }

        [MenuItem("absencee_/absent-console/Refresh Methods")]
        static void RefreshMethods()
        {
            ConsoleEventDatabase.RefreshMethods(true);
        }
    }

}