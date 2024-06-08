using UnityEngine;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// A class representing an argument of a command.
    /// </summary>
    [System.Serializable]
    public class Argument
    {
        public enum ArgumentValueType
        {
            Integer = 0,
            FloatingPoint = 1,
            String = 2,
            Boolean = 4,
            [InspectorName("On|Off")] OnOff = 5,
            Custom = 6,
        }

        [SerializeField] private string m_name = "!null";
        [SerializeField] private ArgumentValueType m_valueType = ArgumentValueType.Boolean;

        public string Name => m_name;
        public ArgumentValueType ValueType => m_valueType;
    }
}