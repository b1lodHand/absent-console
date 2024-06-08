using System.Reflection;
using UnityEngine;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// The static class which contains some helper functions for console system.
    /// </summary>
    public static class ConsoleUtility
    {
        public const string INPUT_ON_LC = "on";
        public const string INPUT_OFF_LC = "off";
        
        public static readonly char[] STRING_WRAPPER_CHARS = {'\'', '\"', '`'};

        public static bool InvokeCommand(Command command, object[] args)
        {
            string methodPreview = command.MethodPreview;

            if (!ConsoleEventHandler.PreviewsOfMethodsInBuild.Contains(methodPreview)) return false;

            try
            {
                MethodInfo targetMethod = ConsoleEventHandler.MethodsInBuild[ConsoleEventHandler.PreviewsOfMethodsInBuild.IndexOf(methodPreview)];
                if (command.Arguments.Count > 0) targetMethod.Invoke(null, args);
                else targetMethod.Invoke(null, null);

                return true;
            }

            catch
            {
                return false;
            }
        }
        public static bool IsValidArgumentInput(Argument.ArgumentValueType originalValueType, string argInput, string forKeyword, out object parsedValue)
        {
            parsedValue = null;
            bool result = false;
            switch (originalValueType)
            {
                case Argument.ArgumentValueType.Integer:
                    result = int.TryParse(argInput, out int intVal);
                    if (result) parsedValue = intVal;
                    break;

                case Argument.ArgumentValueType.FloatingPoint:
                    result = float.TryParse(argInput, out float floatVal);
                    if (result) parsedValue = floatVal;
                    break;

                case Argument.ArgumentValueType.Boolean:
                    result = bool.TryParse(argInput, out bool boolVal);
                    if (result) parsedValue = boolVal;
                    break;

                case Argument.ArgumentValueType.String:
                    result = TryParseStringInput(argInput, out string stringVal);
                    if (result) parsedValue = stringVal;
                    break;

                case Argument.ArgumentValueType.OnOff:
                    string casedInput = argInput.Trim().ToLower();
                    result = ((casedInput == INPUT_ON_LC) || (casedInput == INPUT_OFF_LC));
                    if (result) parsedValue = (casedInput == INPUT_ON_LC);
                    break;

                case Argument.ArgumentValueType.Custom:
                    result = TryParseCustomInput(argInput, forKeyword, out object customVal);
                    if (result) parsedValue = customVal;
                    break;

                default:
                    parsedValue = null;
                    result = false;
                    Console.LogError("Something gone wrong parsing the input");
                    break;
            }

            return result;
        }

        static bool TryParseStringInput(string input, out string parsedString)
        {
            input = input.Trim();

            bool result = false;
            char targetChar = ' ';
            for (int i = 0; i < STRING_WRAPPER_CHARS.Length; i++)
            {
                char current = STRING_WRAPPER_CHARS[i];
                if (input.StartsWith(current) && input.EndsWith(current))
                {
                    result = true;
                    targetChar = current;
                }
            }

            parsedString = input.Trim(targetChar);

            return result;
        }
        static bool TryParseCustomInput(string argInput, string forKeyword, out object parsedValue)
        {
            // Override to have custom logic.
            Console.LogError("You haven't implemented any logic for custom arguments.");

            parsedValue = null;
            return false;
        }
    }

}