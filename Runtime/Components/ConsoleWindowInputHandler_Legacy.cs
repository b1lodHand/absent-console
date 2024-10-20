using com.absence.consolesystem.imported;
using com.absence.consolesystem.internals;
using UnityEngine;

namespace com.absence.consolesystem
{
    /// <summary>
    /// A small component which lets the current console window to receive input. (Uses Unity's 
    /// old input system.)
    /// </summary>
    [AddComponentMenu("absencee_/absent-console/Console Window Input Handler (Legacy)")]
    public class ConsoleWindowInputHandler_Legacy : ConsoleWindowInputHandlerBase
    {
        [SerializeField, Space(10)] private KeyCode m_keyToOpen = KeyCode.Tab;

        private void Update()
        {
            ConsoleWindow target = (m_selectionType == ConsoleWindowSelectionType.Singleton) ? ConsoleWindow.Instance : m_targetConsoleWindow;

            if (Input.GetKeyDown(m_keyToOpen)) target.SwitchWindowVisibility();

            if (!target.IsOpen) return;

            if (Input.GetKeyDown(KeyCode.Return)) target.Push();

            if (Input.GetKeyDown(KeyCode.UpArrow)) target.LoadLastCommand();
        }
    }
}
