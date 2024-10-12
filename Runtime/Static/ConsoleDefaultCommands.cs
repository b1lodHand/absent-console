using System.Collections.Generic;
using System.Text;

namespace com.absence.consolesystem.internals
{
    /// <summary>
    /// The static class which contains the default commands that come with the package.
    /// </summary>
    public static class ConsoleDefaultCommands
    {
        [Command]
        public static void no_methods_selected()
        {
            Console.Log("There are no methods selected for this command. ");
        }

        [Command]
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

        [Command]
        static void Help(string commandName)
        {
            commandName = commandName.Trim();
            List<Command> commandsFound = ConsoleWindow.Instance.GetCommandsWithTheKeyword(commandName);

            if (commandsFound.Count == 0)
            {
                Console.LogError("No commands with the specified keyword found in the build.");
                return;
            }

            StringBuilder sb = new();

            sb.Append($"'{commandName}' search results: ");

            commandsFound.ForEach(command =>
            {
                sb.Append("\n\t");
                sb.Append(Help_GenerateDesciption(command));
            });

            Console.Log(sb.ToString());
        }

        static string Help_GenerateDesciption(Command command)
        {
            string description = ConsoleWindowUtility.WrapWithColorTag(command.Description, ConsoleWindowUtility.DESCRIPTION_COLOR);
            return $"{ConsoleWindowUtility.GeneratePreviewForCommand(command, true)}: {description}";
        }
    }
}
