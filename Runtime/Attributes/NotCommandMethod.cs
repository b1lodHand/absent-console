using System;

namespace com.absence.consolesystem.internals
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class NotCommandMethod : Attribute
    {
        public NotCommandMethod()
        {
        }
    }
}