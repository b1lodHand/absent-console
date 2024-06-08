using com.absence.consolesystem.internals;
using System.Collections.Generic;
using UnityEngine;

namespace com.absence.consolesystem
{
    /// <summary>
    /// The scriptable object responsible for holding commands.
    /// </summary>
    [CreateAssetMenu(menuName = "absencee_/Console Profile", fileName = "New Console Profile")]
    public class ConsoleProfile : ScriptableObject
    {
        [SerializeField] private List<Command> m_commands = new();

        public List<Command> Commands => m_commands;      
    }

}