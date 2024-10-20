using com.absence.consolesystem.internals;
using System.Collections.Generic;
using UnityEngine;

namespace com.absence.consolesystem
{
    /// <summary>
    /// The scriptable object responsible for holding commands.
    /// </summary>
    [CreateAssetMenu(menuName = "absencee_/absent-console/Console Profile (Empty)", fileName = "New Empty Console Profile")]
    public class ConsoleProfile : ScriptableObject
    {
        [SerializeField] private List<Command> m_commands = new();

        public List<Command> Commands => m_commands;      
    }

}