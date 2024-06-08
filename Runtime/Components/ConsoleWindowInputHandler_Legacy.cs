using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.absence.consolesystem
{
    [DisallowMultipleComponent]
    [AddComponentMenu("absencee_/absent-console/Console Window Input Handler (Legacy)")]
    public class ConsoleWindowInputHandler_Legacy : MonoBehaviour
    {
        #region Static Instance
        private static ConsoleWindowInputHandler_Legacy m_instance;
        public static ConsoleWindowInputHandler_Legacy Instance => m_instance;
        #endregion

        [SerializeField] private KeyCode m_keyToOpen = KeyCode.Tab;

        private void Awake()
        {
            if (m_instance == null) m_instance = this;
        }

        private void Start()
        {
            if (ConsoleWindow.Instance == null)
            {
                Debug.Log("There are no consoles to pipe input. Disabling input handler.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (Instance != this)
            {
                if (Instance != null) Debug.Log("There are multiple console window input handlers in the scene.");
                else m_instance = this;
            }

            if (Input.GetKeyDown(m_keyToOpen)) ConsoleWindow.Instance.SwitchWindowVisibility();

            if (!ConsoleWindow.Instance.IsOpen) return;

            if (Input.GetKeyDown(KeyCode.Return)) ConsoleWindow.Instance.RetrieveEnterInput();

            if (Input.GetKeyDown(KeyCode.UpArrow)) ConsoleWindow.Instance.LoadLastCommand();
        }
    }
}
