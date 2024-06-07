using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System;
using UnityEngine;
using System.Linq;

namespace com.absence.consolesystem.internals
{
    public static class ConsoleEventDatabase
    {
        public const bool DEBUG_MODE = true;
        const string DEFAULT_METHOD_NAME = nameof(ConsoleEventHandler.no_methods_selected);
        static MethodInfo DefaultMethod => MethodsInBuild.Where(method => method.Name == DEFAULT_METHOD_NAME).FirstOrDefault();

        const BindingFlags METHOD_FLAGS = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        private static List<MethodInfo> m_methodsInBuild;
        private static List<string> m_methodPreviews;

        public static List<MethodInfo> MethodsInBuild => m_methodsInBuild;
        public static List<string> PreviewsOfMethodsInBuild => m_methodPreviews;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void RefreshMethods_Auto()
        {
            RefreshMethods(DEBUG_MODE);
        }

        public static void RefreshMethods(bool debugMode = false)
        {
            m_methodsInBuild = new();
            m_methodPreviews = new();

            List<MethodInfo> temp = typeof(ConsoleEventHandler).GetMethods(METHOD_FLAGS).ToList();
            temp = temp.Where(method => !method.IsGenericMethod).ToList();
            temp = temp.Where(method => method.GetCustomAttributes(typeof(NotCommandMethod), false).Length == 0).ToList();

            if (temp.Count == 0) return;

            m_methodsInBuild = new(temp);
            m_methodPreviews = temp.ConvertAll(method => GenerateMethodPreview(method));

            if (!debugMode) return;

            StringBuilder debugMessage = new();

            debugMessage.Append("<b>[CONSOLE]: Methods loaded:</b> ");

            m_methodPreviews.ForEach(methodPreview =>
            {
                debugMessage.Append("\n\t-> ");
                debugMessage.Append(methodPreview);
            });

            Debug.Log(debugMessage.ToString());
        }

        public static string GenerateMethodPreview(MethodInfo method)
        {
            StringBuilder sb = new();

            sb.Append(method.Name);
            sb.Append("(");

            List<ParameterInfo> parameters = method.GetParameters().ToList();

            parameters.ForEach(parameter =>
            {
                sb.Append(parameter.ParameterType.Name);
                sb.Append(" ");
                sb.Append(parameter.Name);

                if (parameters.IndexOf(parameter) != parameters.Count - 1) sb.Append(", ");
            });

            sb.Append(")");

            return sb.ToString();
        }

        public static List<MethodInfo> GetSuitableMethodsForCommand(Command command)
        {
            List<MethodInfo> temp = MethodsInBuild.Where(method => method.GetParameters().Length == command.Arguments.Count).ToList();
            if (!temp.Any(method => method.Name == DEFAULT_METHOD_NAME)) temp.Insert(0, DefaultMethod);

            if (command.Arguments.Count == 0) return temp;

            List<Argument> commandArgs = command.Arguments;
            temp = temp.Where(method =>
            {
                if (method == DefaultMethod) return true;

                List<ParameterInfo> methodParams = method.GetParameters().ToList();

                bool result = true;
                for (int i = 0; i < command.Arguments.Count; i++)
                {
                    ParameterInfo param = methodParams[i];
                    Argument arg = commandArgs[i];

                    if (!IsValidArgumentType(arg.ValueType, param.ParameterType)) result = false;
                }

                return result;
            }).ToList();

            return temp;
        }

        public static bool IsValidArgumentType(Argument.ArgumentValueType originalValueType, Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return (originalValueType == Argument.ArgumentValueType.Integer);

                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return (originalValueType == Argument.ArgumentValueType.FloatingPoint);

                case TypeCode.Boolean:
                    return ((originalValueType == Argument.ArgumentValueType.Boolean) || (originalValueType == Argument.ArgumentValueType.OnOff));

                case TypeCode.String:
                    return (originalValueType == Argument.ArgumentValueType.String);

                default:
                    return (originalValueType == Argument.ArgumentValueType.Custom);
            }
        }
    }

}