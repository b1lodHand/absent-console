/*
 Copyright 2024 absencee_

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the “Software”), to deal in the 
Software without restriction, including without limitation the rights to use, copy, modify, 
merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to 
whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */


using System;
using UnityEngine;

namespace com.absence.consolesystem.imported
{
    /// <summary>
    /// Abstract class that provides conditional attributes a base.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal abstract class BaseIfAttribute : PropertyAttribute
    {
        public enum OutputMethod
        {
            ShowHide = 0,
            EnableDisable = 1,
        }

        public string controlPropertyName { get; private set; }
        public object targetValue { get; private set; }
        public bool directBool { get; private set; }
        public bool invert { get; protected set; }
        public OutputMethod outputMethod { get; protected set; }

        public BaseIfAttribute(string comparedPropertyName)
        {
            this.controlPropertyName = comparedPropertyName;
            this.targetValue = null;

            directBool = true;
        }

        public BaseIfAttribute(string comparedPropertyName, object targetValue)
        {
            this.controlPropertyName = comparedPropertyName;
            this.targetValue = targetValue;

            directBool = false;
        }
    }
}
