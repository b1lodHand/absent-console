using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// A script responsible for handling the creation of a console profile.
    /// </summary>
    public static class ConsoleProfileCreationHandler
    {
        [MenuItem("Assets/Create/absencee_/absent-console/Console Profile", priority = 0)]
        static void CreateConsoleProfile_MenuItem()
        {
            string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (selectedPath == string.Empty) return;

            while ((!AssetDatabase.IsValidFolder(selectedPath)))
            {
                TrimLastSlash(ref selectedPath);
            }

            CreateConsoleProfileEndNameEditAction create = ScriptableObject.CreateInstance<CreateConsoleProfileEndNameEditAction>();
            var path = Path.Combine(selectedPath, "New Console Profile.asset");
            var icon = EditorGUIUtility.IconContent("d_ScriptableObject Icon").image as Texture2D;

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, create, path, icon, null);
        }

        private static void TrimLastSlash(ref string path)
        {
            int lastSlashIndex;
            for (lastSlashIndex = path.Length - 1; lastSlashIndex > 0; lastSlashIndex--)
            {
                if (path[lastSlashIndex] == '/') break;
            }

            path = path.Remove(lastSlashIndex, (path.Length - lastSlashIndex));
        }

        public static ConsoleProfile CreateConsoleProfile(string pathName)
        {
            ConsoleProfile exampleProfile = AssetDatabase.LoadAssetAtPath<ConsoleProfile>("Packages/com.absence.consolesystem/Runtime/Examples/ExampleConsoleProfile.asset");
            var itemCreated = ScriptableObject.Instantiate(exampleProfile);
            AssetDatabase.CreateAsset(itemCreated, pathName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = itemCreated;

            return itemCreated;
        }

        internal class CreateConsoleProfileEndNameEditAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                ConsoleProfileCreationHandler.CreateConsoleProfile(pathName);
            }

            public override void Cancelled(int instanceId, string pathName, string resourceFile)
            {
                ConsoleProfile item = EditorUtility.InstanceIDToObject(instanceId) as ConsoleProfile;
                ScriptableObject.DestroyImmediate(item);
            }
        }
    }

}