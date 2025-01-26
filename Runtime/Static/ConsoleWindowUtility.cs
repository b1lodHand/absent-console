using System.Collections.Generic;
using System.Text;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// The static class which contains some helper functions for console window.
    /// </summary>
    public static class ConsoleWindowUtility
    {
        const string k_boolParamColor = "#4d94ff";
        const string k_integerParamColor = "#afff99";
        const string k_floatParamColor = "#ffff80";
        const string k_stringParamColor = "#80ffff";
        //const string k_customParamColor = "#ff99e6";

        public static readonly Dictionary<Argument.ArgumentValueType, string> ColorTags = new()
        {
            { Argument.ArgumentValueType.Integer, k_integerParamColor },
            { Argument.ArgumentValueType.Float, k_floatParamColor },
            { Argument.ArgumentValueType.String, k_stringParamColor },
            { Argument.ArgumentValueType.Boolean, k_boolParamColor },
            { Argument.ArgumentValueType.OnOff, k_boolParamColor },
        };

        const string k_white = "#ffffff";

        public const string ERROR_COLOR = "#ff4d4d";
        public const string WARNING_COLOR = "#ffee80";
        public const string LOG_COLOR = k_white;
        public const string DESCRIPTION_COLOR = "#bfbfbf";

        /// <summary>
        /// Use to generate a preview from a command.
        /// </summary>
        /// <param name="command">Command to generate preview from.</param>
        /// <param name="richText">If true, some areas in the result will be wrapped with color tags.</param>
        /// <returns>Returns the preview generated.</returns>
        public static string GeneratePreviewForCommand(Command command, bool richText = false)
        {
            StringBuilder preview = new();

            if (richText) preview.Append("<color=white>");

            preview.Append(command.Keyword.Trim());

            if (richText) preview.Append("</color>");

            if (command.Arguments.Count == 0) return preview.ToString();

            for (int i = 0; i < command.Arguments.Count; i++)
            {
                preview.Append(" ");
                preview.Append(GeneratePreviewForParameter(command.Arguments[i], richText));
            }

            return preview.ToString();
        }
        /// <summary>
        /// Use to generate a preview from a parameter.
        /// </summary>
        /// <param name="command">Parameter to generate preview from.</param>
        /// <param name="richText">If true, some areas in the result will be wrapped with color tags.</param>
        /// <returns>Returns the preview generated.</returns>
        public static string GeneratePreviewForParameter(Argument parameter, bool richText = false)
        {
            StringBuilder preview = new();

            if (richText)
            {
                string colorTag = GetColorTag(parameter.ValueType);
                preview.Append($"<color={colorTag}>");
            }

            preview.Append("[");

            preview.Append(parameter.Name.Trim());
            preview.Append("]");

            if (richText) preview.Append("</color>");

            return preview.ToString();
        }
        /// <summary>
        /// Use to generate a detailed description from a command.
        /// </summary>
        /// <param name="command">Command to generate description from.</param>
        /// <returns>Returns the description generated.</returns>
        public static string GenerateDetailedDescriptionForCommand(Command command)
        {
            StringBuilder sb = new();

            sb.Append(command.Description);

            if (string.IsNullOrWhiteSpace(command.MethodPreview)) return sb.ToString();

            sb.Append("\n\n");
            sb.Append($"Calls: '{command.MethodPreview}'");

            return sb.ToString();
        }

        /// <summary>
        /// Use to generate a helper for argument type colors  in a text.
        /// </summary>
        /// <returns>Returns the generated helper text.</returns>
        public static string GenerateArgumentColorHelper()
        {
            StringBuilder sb = new();

            foreach (var kvp in ConsoleWindowUtility.ColorTags)
            {
                sb.Append("<b>");
                sb.Append(ConsoleWindowUtility.WrapWithColorTag("[]", kvp.Value));
                sb.Append("</b> ");
                sb.Append(kvp.Key.ToString());
                sb.Append(", ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Use to wrap a string with XML color tags, specifying the color.
        /// </summary>
        /// <param name="messageToWrap">Message to wrap.</param>
        /// <param name="colorTag">Hex code of the color wanted.</param>
        /// <returns>Returns the wrapped version of the message.</returns>
        public static string WrapWithColorTag(string messageToWrap, string colorTag)
        {
            StringBuilder sb = new();
            sb.Append($"<color={colorTag}>");
            sb.Append(messageToWrap);
            sb.Append("</color>");

            return sb.ToString();
        }

        /// <summary>
        /// Use to get the hex color code of an argument value type.
        /// </summary>
        /// <param name="valueType">The argument value type.</param>
        /// <returns>Returns the hex code found.</returns>
        private static string GetColorTag(Argument.ArgumentValueType valueType)
        {
            return ColorTags[valueType];
        }
    }

}