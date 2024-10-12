using System;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// Use this attribute to mark any static method as a command to include it in the build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
        }
    }
}