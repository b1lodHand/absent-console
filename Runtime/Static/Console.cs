using com.absence.consolesystem.internals;

namespace com.absence.consolesystem
{
    /// <summary>
    /// Wrapper class for <see cref="ConsoleWindow.Instance"/>.
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// Writes a message to <see cref="ConsoleWindow.Instance"/>.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="extraLineBreak">>If true, an extra vertical space (\n) is added to the end of the message written.</param>
        public static void Log(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.LOG_COLOR), extraLineBreak);
        }

        /// <summary>
        /// Writes a warning to <see cref="ConsoleWindow.Instance"/>.
        /// </summary>
        /// <param name="message">Warning to write.</param>
        /// <param name="extraLineBreak">>If true, an extra vertical space (\n) is added to the end of the message written.</param>
        public static void LogWarning(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.WARNING_COLOR), extraLineBreak);
        }

        /// <summary>
        /// Writes an error to <see cref="ConsoleWindow.Instance"/>.
        /// </summary>
        /// <param name="message">Error to write.</param>
        /// <param name="extraLineBreak">>If true, an extra vertical space (\n) is added to the end of the message written.</param>
        public static void LogError(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.ERROR_COLOR), extraLineBreak);
        }

        /// <summary>
        /// Opens <see cref="ConsoleWindow.Instance"/>.
        /// </summary>
        public static void Open()
        {
            ConsoleWindow.Instance.OpenWindow();
        }

        /// <summary>
        /// Closes <see cref="ConsoleWindow.Instance"/>.
        /// </summary>
        /// <param name="clearInputField">If true, input field gets cleared upon the closing process.</param>
        public static void Close(bool clearInputField)
        {
            ConsoleWindow.Instance.CloseWindow(clearInputField);
        }
    }
}