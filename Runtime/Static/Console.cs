using com.absence.consolesystem.internals;

namespace com.absence.consolesystem
{
    public static class Console
    {
        public static void Log(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.LOG_COLOR), extraLineBreak);
        }

        public static void LogWarning(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.WARNING_COLOR), extraLineBreak);
        }

        public static void LogError(string message, bool extraLineBreak = true)
        {
            ConsoleWindow.Instance.Write(ConsoleWindowUtility.WrapWithColorTag(message, ConsoleWindowUtility.ERROR_COLOR), extraLineBreak);
        }
    }

}