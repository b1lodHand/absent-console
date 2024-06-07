using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.absence.consolesystem
{
    [CreateAssetMenu(menuName = "absencee_/Console Profile", fileName = "New Console Profile")]
    public class ConsoleProfile : ScriptableObject
    {
        [SerializeField] private List<Command> m_commands = new();

        public List<Command> Commands => m_commands;

        public List<Command> GetCommandsWithTheKeyword(string keyword)
        {
            keyword = keyword.Trim();

            return Commands.Where(command => command.Keyword == keyword).ToList();
        }
    }

}