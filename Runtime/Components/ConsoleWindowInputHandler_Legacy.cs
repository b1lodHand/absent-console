using UnityEngine;

namespace com.absence.consolesystem
{
    /// <summary>
    /// A small component which lets the current console window to receive input. (Uses Unity's 
    /// old input system.)
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("absencee_/absent-console/Console Window Input Handler (Legacy)")]
    public class ConsoleWindowInputHandler_Legacy : MonoBehaviour
    {
        [SerializeField] private KeyCode m_keyToOpen = KeyCode.Tab;

        private void Start()
        {
            if (ConsoleWindow.Instance == null)
            {
                Debug.Log("There are no consoles to send input. Disabling input handler.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(m_keyToOpen)) ConsoleWindow.Instance.SwitchWindowVisibility();

            if (!ConsoleWindow.Instance.IsOpen) return;

            if (Input.GetKeyDown(KeyCode.Return)) ConsoleWindow.Instance.Push();

            if (Input.GetKeyDown(KeyCode.UpArrow)) ConsoleWindow.Instance.LoadLastCommand();
        }
    }
}
