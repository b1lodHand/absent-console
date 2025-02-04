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
    public static class ConsoleEventDatabase
    {
        public const bool AUTO_LOAD_METHODS = true;
        public const bool DEBUG_MODE = false;

        const string DEFAULT_METHOD_NAME = nameof(ConsoleDefaultCommands.no_methods_selected);
        public static MethodInfo DefaultMethod {  get; private set; }

        const BindingFlags METHOD_FLAGS = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        static List<MethodInfo> s_methodsInBuild;
        static List<string> s_methodPreviews;
        static Dictionary<string, MethodInfo> s_reversedPreviewPairs;

        public static List<MethodInfo> MethodsInBuild => s_methodsInBuild;
        public static List<string> PreviewsOfMethodsInBuild => s_methodPreviews;
        public static Dictionary<string, MethodInfo> ReversedPreviewPairs => s_reversedPreviewPairs;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        static void RefreshMethods_Auto()
        {
#pragma warning disable CS0162 // Unreachable code detected
            if (!AUTO_LOAD_METHODS) return;

            RefreshMethods(DEBUG_MODE);
#pragma warning restore CS0162 // Unreachable code detected
        }

        /// <summary>
        /// Use to refresh and find the methods in the build using reflection.
        /// </summary>
        /// <param name="debugMode">If true, result will be printed into Unity's console.</param>
        public static void RefreshMethods(bool debugMode = false)
        {
            s_methodsInBuild = new();
            s_methodPreviews = new();
            s_reversedPreviewPairs = new();

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

            s_methodsInBuild = temp;

            s_methodsInBuild.ForEach(method =>
            {
                if (method.Name.Equals(DEFAULT_METHOD_NAME)) DefaultMethod = method; 
                s_methodPreviews.Add(GenerateMethodPreview(method));
            });

            if (!debugMode) return;

            Debug.Log(PrintMethodList());
        }
        
        /// <summary>
        /// Use to print the current list of methods included in the build.
        /// </summary>
        /// <returns></returns>
        public static string PrintMethodList()
        {
            StringBuilder debugMessage = new();

            debugMessage.Append("<b>[CONSOLE] Methods loaded:</b> ");

            s_methodPreviews.ForEach(methodPreview =>
            {
                debugMessage.Append("\n\t");
                debugMessage.Append("-> ");
                debugMessage.Append("<color=white>");
                debugMessage.Append(methodPreview);
                debugMessage.Append("</color>");
            });

            return debugMessage.ToString();
        }

        /// <summary>
        /// Use to generate a preview of a method.
        /// </summary>
        /// <param name="method">Method to generate preview from.</param>
        /// <returns>The preview generated as string.</returns>
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
            string preview = sb.ToString();

            if (!s_reversedPreviewPairs.ContainsKey(preview))
                s_reversedPreviewPairs.Add(preview, method);

            return preview;
        }

        /// <summary>
        /// Use to get all suitable methods in the build for a specific command.
        /// </summary>
        /// <param name="command">The specific command.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Use to check if a type is valid for a specific type of built-in argument value type.
        /// </summary>
        /// <param name="originalValueType">Original argument value type.</param>
        /// <param name="type">Type to check.</param>
        /// <returns></returns>
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
                    return (originalValueType == Argument.ArgumentValueType.Float);

                case TypeCode.Boolean:
                    return ((originalValueType == Argument.ArgumentValueType.Boolean) || (originalValueType == Argument.ArgumentValueType.OnOff));

                case TypeCode.String:
                    return (originalValueType == Argument.ArgumentValueType.String);

                default:
                    //return (originalValueType == Argument.ArgumentValueType.Custom);
                    return false;
            }
        }
    }

}