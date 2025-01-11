using com.absence.consolesystem.internals;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace com.absence.consolesystem.editor
{
    /// <summary>
    /// The static class responsible for managing the editor-side tasks of the console system.
    /// </summary>
    [InitializeOnLoad]
    public static class EditorJobsHelper
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

        [MenuItem("GameObject/absencee_/absent-console/Create Example Console Window", priority = 0)]
        static void CreateExampleWindow()
        {
            GameObject objectToLoad = AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.absence.consolesystem/Runtime/Examples/ConsoleContainer.prefab");
            GameObject objectLoaded = GameObject.Instantiate(objectToLoad);
            Selection.activeGameObject = objectLoaded;

            Undo.RegisterCreatedObjectUndo(objectLoaded, "Created Example Console Window");

            Debug.LogWarning("This Console Window needs EventSystem to work. Make sure to create one before starting the game.");

            EditorApplication.delayCall += () =>
            {
                Type sceneHierarchyType = Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor");
                EditorWindow hierarchyWindow = EditorWindow.GetWindow(sceneHierarchyType);
                hierarchyWindow.SendEvent(EditorGUIUtility.CommandEvent("Rename"));
            };
        }
    }

}