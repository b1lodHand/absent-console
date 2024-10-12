#if ENABLE_INPUT_SYSTEM

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
        [SerializeField] private Key m_keyToOpen = Key.Tab;

        private void Start()
        {
            if (ConsoleWindow.Instance == null)
            {
                Debug.Log("There are no consoles to send/receive input. Disabling input handler.");
                enabled = false;
            }
        }

        private void Update()
        {
            KeyControl openKeyCtrl =
                Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == m_keyToOpen);

            KeyControl submitKeyCtrl =
                Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == Key.Enter);

            KeyControl copyKeyCtrl =
                Keyboard.current.allKeys.FirstOrDefault(keyCtrl => keyCtrl.keyCode == Key.UpArrow);

            if (openKeyCtrl != null && openKeyCtrl.isPressed) ConsoleWindow.Instance.SwitchWindowVisibility();

            if (!ConsoleWindow.Instance.IsOpen) return;

            if (submitKeyCtrl != null && submitKeyCtrl.isPressed) ConsoleWindow.Instance.Push();

            if (copyKeyCtrl != null && copyKeyCtrl.isPressed) ConsoleWindow.Instance.LoadLastCommand();
        }
    }
}

#endif