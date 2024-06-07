using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace com.absence.consolesystem.internals
{
    /* Define logic for your commands here.*/
    public static class ConsoleEventHandler
    {
        // It is recommended to remain this method as is. It is the default method for the commands without any method specified.
        public static void no_methods_selected()
        {
            Console.Log("There are no methods selected for this command. ");
        }

        static void Help()
        {
            List<Command> commandsAvailable = ConsoleWindow.Instance.Profile.Commands;

            StringBuilder sb = new();

            sb.Append("Here are the all commands in the build: ");

            commandsAvailable.ForEach(command =>
            {
                sb.Append("\n\t");
                sb.Append(Help_GenerateDesciption(command));
            });

            Console.Log(sb.ToString());
        }

        static void Help(string commandName)
        {
            List<Command> commandsFound = ConsoleWindow.Instance.Profile.GetCommandsWithTheKeyword(commandName);

            if (commandsFound.Count == 0)
            {
                Console.LogError("No commands with the specified keyword found in the build.");
                return;
            }

            StringBuilder sb = new();

            sb.Append("Help search results: ");

            commandsFound.ForEach(command =>
            {
                sb.Append("\n\t");
                sb.Append(Help_GenerateDesciption(command));
            });

            Console.Log(sb.ToString());
        }

        [NotCommandMethod]
        static string Help_GenerateDesciption(Command command)
        {
            string description = ConsoleWindowUtility.WrapWithColorTag(command.Description, ConsoleWindowUtility.DESCRIPTION_COLOR);
            return $"{ConsoleWindowUtility.GeneratePreviewForCommand(command, true)}: {description}";
        }

        static void Help(int id)
        {
            Console.Log($"ID of this member is: {id}");
        }

        static void GodModeSwitch()
        {
            Debug.Log("god mode switched.");
        }

        static void GodModeSwitch2()
        {
            Debug.Log("god mode switched.");
        }

        static void GodMode(bool state)
        {
            if (state) Debug.Log("god mode on");
            else Debug.Log("god mode off");
        }
    }
}
