using com.absence.consolesystem.imported;
using UnityEngine;

namespace com.absence.consolesystem.internals
{
    [DisallowMultipleComponent]
    public class ConsoleWindowInputHandlerBase : MonoBehaviour
    {
        [SerializeField] protected ConsoleWindowSelectionType m_selectionType = ConsoleWindowSelectionType.Singleton;

        [SerializeField,
    HideIf(nameof(m_selectionType), ConsoleWindowSelectionType.Singleton, order = 0),
    EnableIf(nameof(m_selectionType), ConsoleWindowSelectionType.Manual, order = 1)]
        protected ConsoleWindow m_targetConsoleWindow;

        [ContextMenu("Find Attached Console Window")]
        void FindAttachedWindow_MenuItem()
        {
            FindAttachedWindow();
        }

        [ContextMenu("Find Attached Console Window", true)]
        protected virtual bool FindAttachedWindow_MenuValidation()
        {
            return m_selectionType == ConsoleWindowSelectionType.AutoOnSameObject;
        }

        protected virtual void OnValidate()
        {
            FindAttachedWindow();
        }

        protected virtual void FindAttachedWindow()
        {
            if (m_selectionType != ConsoleWindowSelectionType.AutoOnSameObject) return;

            if (TryGetComponent(out ConsoleWindow windowFound))
            {
                m_targetConsoleWindow = windowFound;
            }
        }
    }
}
