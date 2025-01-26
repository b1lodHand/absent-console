using System;
using System.Reflection;
using UnityEngine;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// The static class which contains some helper functions for console system.
    /// </summary>
    public static class ConsoleUtility
    {
        public enum ArgumentValidityCheckMode
        {
            Soft = 0,
            Hard = 1,
        }

        public const string INPUT_ON_LC = "on";
        public const string INPUT_OFF_LC = "off";

        /// <summary>
        /// Use to invoke a command.
        /// </summary>
        /// <param name="command">Command to invoke.</param>
        /// <param name="args">Arguments, if needed.</param>
        /// <returns></returns>
        public static bool InvokeCommand(ConsoleWindow sender, Command command, object[] args)
        {
            string methodPreview = command.MethodPreview;

            if (!ConsoleEventDatabase.PreviewsOfMethodsInBuild.Contains(methodPreview)) return false;

            try
            {
                ConsoleWindow.Sender = sender;

                MethodInfo targetMethod = ConsoleEventDatabase.ReversedPreviewPairs[methodPreview];
                if (command.Arguments.Count > 0 && (!targetMethod.Equals(ConsoleEventDatabase.DefaultMethod)))
                    targetMethod.Invoke(null, args);
                else 
                    targetMethod.Invoke(null, null);

                ConsoleWindow.Sender = null;
                return true;
            }

            catch (Exception e)
            {
                Debug.LogException(e);
                ConsoleWindow.Sender = null;
                return false;
            }
        }

        /// <summary>
        /// Check if an argument input is valid.
        /// </summary>
        /// <param name="originalValueType">Original argument value type to use.</param>
        /// <param name="argInput">Raw argument input.</param>
        /// <param name="forKeyword">Keyword to limit the scope of the check.</param>
        /// <param name="parsedValue">Output of the parsed value of the argument.</param>
        /// <returns></returns>
        public static bool IsValidArgumentInput(ConsoleWindow sender, Argument.ArgumentValueType originalValueType, string argInput, ArgumentValidityCheckMode checkMode, out object parsedValue)
        {
            argInput = argInput.Trim();

            parsedValue = null;
            bool result = false;

            if (checkMode == ArgumentValidityCheckMode.Hard)
            {
                string casedInput = argInput.ToLower();

                bool canBeInt = int.TryParse(argInput, out int intValue);
                bool canBeFloat = float.TryParse(argInput, out float floatValue);
                bool canBeBoolean = bool.TryParse(argInput, out bool boolValue);
                bool canBeOnOff = ((casedInput == INPUT_ON_LC) || (casedInput == INPUT_OFF_LC));
                bool onOffValue = (casedInput == INPUT_ON_LC);

                if (originalValueType == Argument.ArgumentValueType.String)
                {
                    result = !(canBeInt || canBeFloat || canBeBoolean || canBeOnOff);
                    if (result) parsedValue = argInput;
                }

                else if (originalValueType == Argument.ArgumentValueType.Float)
                {
                    result = canBeFloat && !canBeInt;
                    if (result) parsedValue = floatValue;
                }

                else if (originalValueType == Argument.ArgumentValueType.Integer)
                {
                    result = canBeInt;
                    if (result) parsedValue = intValue;
                }

                else if (originalValueType == Argument.ArgumentValueType.Boolean)
                {
                    result = canBeBoolean;
                    if (result) parsedValue = boolValue;
                }

                else if (originalValueType == Argument.ArgumentValueType.OnOff)
                {
                    result = canBeOnOff;
                    if (result) parsedValue = onOffValue;
                }

                return result;
            }

            switch (originalValueType)
            {
                case Argument.ArgumentValueType.Integer:
                    result = int.TryParse(argInput, out int intVal);
                    if (result) parsedValue = intVal;
                    break;

                case Argument.ArgumentValueType.Float:
                    result = float.TryParse(argInput, out float floatVal);
                    if (result) parsedValue = floatVal;
                    break;

                case Argument.ArgumentValueType.Boolean:
                    result = bool.TryParse(argInput, out bool boolVal);
                    if (result) parsedValue = boolVal;
                    break;

                case Argument.ArgumentValueType.String:
                    result = true;
                    if (result) parsedValue = argInput;
                    break;

                case Argument.ArgumentValueType.OnOff:
                    string casedInput = argInput.ToLower();
                    result = ((casedInput == INPUT_ON_LC) || (casedInput == INPUT_OFF_LC));
                    if (result) parsedValue = (casedInput == INPUT_ON_LC);
                    break;

                default:
                    parsedValue = null;
                    result = false;
                    sender.LogError("Something went wrong parsing the input");
                    break;
            }

            return result;
        }
    }

}