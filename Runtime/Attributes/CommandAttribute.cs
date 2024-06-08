using System;

namespace com.absence.consolesystem.internals
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
        }
    }
}