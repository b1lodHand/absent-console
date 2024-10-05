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
        const string k_customParamColor = "#ff99e6";

        const string k_white = "#ffffff";

        public const string ERROR_COLOR = "#ff4d4d";
        public const string WARNING_COLOR = "#ffee80";
        public const string LOG_COLOR = k_white;
        public const string DESCRIPTION_COLOR = "#bfbfbf";

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
        public static string GenerateDetailedDescriptionForCommand(Command command)
        {
            StringBuilder sb = new();

            sb.Append(command.Description);

            if (string.IsNullOrWhiteSpace(command.MethodPreview)) return sb.ToString();

            sb.Append("\n\n");
            sb.Append($"Calls: '{command.MethodPreview}'");

            return sb.ToString();
        }

        public static string WrapWithColorTag(string messageToWrap, string colorTag)
        {
            StringBuilder sb = new();
            sb.Append($"<color={colorTag}>");
            sb.Append(messageToWrap);
            sb.Append("</color>");

            return sb.ToString();
        }

        private static string GetColorTag(Argument.ArgumentValueType valueType)
        {
            switch (valueType)
            {
                case Argument.ArgumentValueType.Integer:
                    return k_integerParamColor;

                case Argument.ArgumentValueType.FloatingPoint:
                    return k_floatParamColor;

                case Argument.ArgumentValueType.String:
                    return k_stringParamColor;

                case Argument.ArgumentValueType.Boolean:
                    return k_boolParamColor;

                case Argument.ArgumentValueType.OnOff:
                    return k_boolParamColor;

                default:
                    return k_customParamColor;
            }
        }
    }

}