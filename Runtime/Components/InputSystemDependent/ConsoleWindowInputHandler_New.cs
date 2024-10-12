#if ENABLE_INPUT_SYSTEM

using com.absence.consolesystem.imported;
using com.absence.consolesystem.internals;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace com.absence.consolesystem
{
    /// <summary>
    /// A small component which lets the current console window to receive input.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("absencee_/absent-console/Console Window Input Handler (New)")]
    public class ConsoleWindowInputHandler_New : MonoBehaviour
    {
        [SerializeField] private ConsoleWindowSelectionType m_selectionType = ConsoleWindowSelectionType.Singleton;
        [SerializeField] private Key m_keyToOpen = Key.Tab;

        [SerializeField,
    HideIf(nameof(m_selectionType), ConsoleWindowSelectionType.Singleton, order = 0),
    EnableIf(nameof(m_selectionType), ConsoleWindowSelectionType.Manual, order = 1)]
        private ConsoleWindow m_targetConsoleWindow;

        KeyControl m_openKeyCtrl;
        KeyControl m_submitKeyCtrl;
        KeyControl m_copyKeyCtrl;

        private void Start()
        {
            m_openKeyCtrl =
            Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == m_keyToOpen);

            m_submitKeyCtrl =
                Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == Key.Enter);

            m_copyKeyCtrl =
                Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == Key.UpArrow);
        }

        private void Update()
        {
            ConsoleWindow target = (m_selectionType == ConsoleWindowSelectionType.Singleton) ? ConsoleWindow.Instance : m_targetConsoleWindow;

            if (m_openKeyCtrl != null && m_openKeyCtrl.wasPressedThisFrame) target.SwitchWindowVisibility();

            if (!target.IsOpen) return;

            if (m_submitKeyCtrl != null && m_submitKeyCtrl.wasPressedThisFrame) target.Push();

            if (m_copyKeyCtrl != null && m_copyKeyCtrl.wasPressedThisFrame) target.LoadLastCommand();
        }

        private void OnValidate()
        {
            if (m_selectionType != ConsoleWindowSelectionType.AutoOnSameObject) return;

            if (TryGetComponent(out ConsoleWindow windowFound))
            {
                m_targetConsoleWindow = windowFound;
            }
        }
    }
}

#endif