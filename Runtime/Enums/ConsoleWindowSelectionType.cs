namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// An enum to determine how to select an input handlers target window.
    /// </summary>
    public enum ConsoleWindowSelectionType
    {
        /// <summary>
        /// Input handler tries to get the singleton window, if available, at runtime.
        /// </summary>
        Singleton = 0,
        /// <summary>
        /// Input handler finds the console window component attached to the same gameObject.
        /// </summary>
        AutoOnSameObject = 1,
        /// <summary>
        /// Lets you to set the target manually.
        /// </summary>
        Manual = 2,
    }
}
