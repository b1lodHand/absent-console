using com.absence.consolesystem.internals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.absence.consolesystem
{
    /// <summary>
    /// The <b>singleton</b> component that takes place in the scene as a console window.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("absencee_/absent-console/Console Window")]
    public class ConsoleWindow : MonoBehaviour
    {
        #region Singleton
        private static ConsoleWindow m_instance;
        public static ConsoleWindow Instance => m_instance;
        #endregion

        [SerializeField] private ConsoleProfile m_profile;
        public ConsoleProfile Profile => m_profile;

        [SerializeField] private bool m_caseSensitive = false;
        public bool IsCaseSensitive => m_caseSensitive;

        private List<Command> m_commands = new();
        public List<Command> Commands => m_commands;

        [Space(10)]

        [Header("UI")]

        [SerializeField] private GameObject m_panel;
        [SerializeField] private InputField m_inputField;
        [SerializeField] private Text m_logText;
        [SerializeField] private ScrollRect m_scrollRect;

        private bool m_open = false;
        public bool IsOpen => m_open;

        private string m_currentCommand;
        private string m_lastCommandInvoked;

#if UNITY_EDITOR
        public bool e_showPreview { get; set; } = true;
#endif

        private void Awake()
        {
            #region Singleton
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
            #endregion

            Transform dontDestroyOnLoadTarget = transform;
            while(dontDestroyOnLoadTarget.parent != null) dontDestroyOnLoadTarget = dontDestroyOnLoadTarget.parent;

            DontDestroyOnLoad(dontDestroyOnLoadTarget.gameObject);

            CloseWindow(true);
            FetchCommands();
        }

        private void FetchCommands()
        {
            Console.Log("Initializing console...", false);

            m_commands = new(m_profile.Commands);

            Console.Log("Done.", false);
            Console.LogWarning($"There are {m_commands.Count} commands found in the build.");
        }
        private bool TryParseInput(string commandInput, out Command foundCommand, out object[] refinedArgs)
        {
            commandInput = commandInput.Trim();

            foundCommand = null;
            refinedArgs = null;

            if (m_commands.Count == 0)
            {
                Console.LogError("There are no commands to filter in the build.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(commandInput))
            {
                Console.LogError("Command input cannot be empty!");
                return false;
            }

            string[] pieces = commandInput.Split(null);
            if (pieces.Length == 0)
            {
                Console.LogError($"There is something wrong with the command input: '{commandInput}'");
                return false;
            }

            string keyword = pieces[0];

            List<Command> commandsWithSameKeyword = GetCommandsWithTheKeyword(keyword);

            if (commandsWithSameKeyword.Count == 0)
            {
                Console.LogError($"Command keyword couldn't found: {keyword}");
                return false;
            }

            int argCount = pieces.Length - 1;
            List<Command> commandsWithMatchingArgCount = commandsWithSameKeyword.Where(command => command.Arguments.Count == argCount).ToList();

            if (commandsWithMatchingArgCount.Count == 0)
            {
                Console.LogError($"There are no '{keyword}' command overloads with {argCount} arguments in the build.");
                return false;
            }
            if (argCount == 0)
            {
                if (commandsWithMatchingArgCount.Count > 1)
                {
                    Console.LogError("There are more than one commands with the same attributes. Check your Command Window's inspector.");
                    return false;
                }

                foundCommand = commandsWithMatchingArgCount.FirstOrDefault();
                return true;
            }

            string[] rawArgs = new string[argCount];
            object[] parsedArgs = new object[argCount];

            for (int i = 0; i < argCount; i++)
            {
                rawArgs[i] = pieces[i + 1];
            }

            List<Command> finalFiltering = commandsWithMatchingArgCount.Where(command =>
            {
                object[] compArgs = new object[argCount];

                bool result = true;
                for (int i = 0; i < command.Arguments.Count; i++)
                {
                    Argument argument = command.Arguments[i];
                    string argumentInput = rawArgs[i];

                    if (!ConsoleUtility.IsValidArgumentInput(argument.ValueType, argumentInput, keyword, out object parsedValue)) result = false;

                    compArgs[i] = parsedValue;
                }

                if (result) parsedArgs = compArgs;
                return result;
            }).ToList();

            if (finalFiltering.Count == 0)
            {
                Console.LogError("One of your arguments' type is wrong.");
                return false;
            }
            if (finalFiltering.Count > 1)
            {
                Console.LogError("There are more than one commands associated with your input in the build. Check your ConsoleWindow's inspector.");
                return false;
            }

            foundCommand = finalFiltering.FirstOrDefault();
            refinedArgs = parsedArgs;
            return true;
        }

        public void Write(string messageToWrite, bool extraLineBreak = true)
        {
            StringBuilder sb = new(m_logText.text);
            sb.Append(messageToWrite);

            sb.Append("\n");
            if (extraLineBreak) sb.Append("\n");

            m_logText.text = sb.ToString();

            Canvas.ForceUpdateCanvases();
            m_scrollRect.verticalNormalizedPosition = 0f;
        }
        public List<Command> GetCommandsWithTheKeyword(string keyword)
        {
            keyword = keyword.Trim();

            return Commands.Where(command =>
            {
                if (m_caseSensitive) return keyword == command.Keyword;
                else return keyword.ToLower() == command.Keyword.ToLower();
            }).ToList();
        }

        public void OpenWindow()
        {
            m_open = true;

            m_panel.SetActive(true);

            SelectInputField();
        }
        public void CloseWindow(bool clearConsole)
        {
            m_open = false;

            m_currentCommand = m_inputField.text;
            if (clearConsole) m_inputField.text = string.Empty;
            EventSystem.current.SetSelectedGameObject(null);

            m_panel.SetActive(false);
        }
        public void SwitchWindowVisibility()
        {
            if (m_open) CloseWindow(false);
            else OpenWindow();
        }
        public void RetrieveEnterInput()
        {
            m_currentCommand = m_inputField.text;
            m_lastCommandInvoked = m_currentCommand;

            if (!TryParseInput(m_currentCommand, out Command command, out object[] args))
            {
                m_currentCommand = string.Empty;
                StartCoroutine(ClearInputField());
                return;
            }

            if (!ConsoleUtility.InvokeCommand(command, args))
            {
                Console.LogError("Something went wrong while invoking the command.");
            }

            m_currentCommand = string.Empty;
            StartCoroutine(ClearInputField());
        }
        public void LoadLastCommand()
        {
            if (EventSystem.current.currentSelectedGameObject != m_inputField.gameObject) return;
            if (string.IsNullOrWhiteSpace(m_lastCommandInvoked)) return;

            m_inputField.text = m_lastCommandInvoked;
            m_inputField.MoveTextEnd(false);
        }


        private void SelectInputField()
        {
            m_inputField.Select();
            StartCoroutine(C_DisableHighlight());
        }
        private IEnumerator ClearInputField()
        {
            EventSystem.current.SetSelectedGameObject(null);

            yield return null;

            m_inputField.text = string.Empty;
            SelectInputField();
        }
        private IEnumerator C_DisableHighlight()
        {
            Color originalTextColor = m_inputField.selectionColor;
            originalTextColor.a = 0f;

            m_inputField.selectionColor = originalTextColor;

            yield return null;

            m_inputField.MoveTextEnd(false);

            originalTextColor.a = 1f;
            m_inputField.selectionColor = originalTextColor;
        }

    }
}