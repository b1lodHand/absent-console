using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System;
using UnityEngine;
using System.Linq;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// The static class responsible for searching and containing static methods needed by commands.
    /// </summary>
    public static class ConsoleEventHandler
    {
        public const bool DEBUG_MODE = false;
        const string DEFAULT_METHOD_NAME = nameof(ConsoleDefaultCommands.no_methods_selected);
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

            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            List<Type> allTypes = new();
            List<MethodInfo> temp = new();

            assemblies.ForEach(asm =>
            {
                List<Type> localTypes = asm.GetTypes().Where(t => t.IsClass).ToList();
                localTypes.ForEach(type => allTypes.Add(type));
            });

            allTypes.ForEach(type =>
            {
                List<MethodInfo> localMethods = type.GetMethods(METHOD_FLAGS).Where(method =>
                {
                    return (!method.IsGenericMethod) && (method.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0);
                }).ToList();

                localMethods.ForEach(method => temp.Add(method));
            });

            if (temp.Count == 0) return;

            m_methodsInBuild = new(temp);
            m_methodPreviews = temp.ConvertAll(method => GenerateMethodPreview(method));

            if (!debugMode) return;

            Debug.Log(PrintMethodList());
        }

        public static string PrintMethodList()
        {
            StringBuilder debugMessage = new();

            debugMessage.Append("<b>[CONSOLE]: Methods loaded:</b> ");

            m_methodPreviews.ForEach(methodPreview =>
            {
                debugMessage.Append("\n\t-> ");
                debugMessage.Append(methodPreview);
            });

            return debugMessage.ToString();
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