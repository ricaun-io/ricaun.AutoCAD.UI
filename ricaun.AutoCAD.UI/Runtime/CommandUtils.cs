using Autodesk.AutoCAD.Runtime;

namespace ricaun.AutoCAD.UI.Runtime
{
    /// <summary>
    /// Provides utility methods for managing AutoCAD commands at runtime.
    /// </summary>
    public static class CommandUtils
    {
        /// <summary>
        /// The default command group name used for command registration.
        /// </summary>
        private const string CommandGroup = "CommandGroup";

        /// <summary>
        /// Determines whether a command with the specified name is defined in AutoCAD.
        /// </summary>
        /// <param name="commandName">The name of the command to check.</param>
        /// <returns><c>true</c> if the command is defined; otherwise, <c>false</c>.</returns>
        public static bool IsCommandDefined(string commandName)
        {
            return Autodesk.AutoCAD.Internal.Utils.IsCommandDefined(commandName);
        }

        /// <summary>
        /// Adds a new command with the specified name and callback action using the default command group and <see cref="CommandFlags.Modal"/>.
        /// </summary>
        /// <param name="commandName">The name of the command to add.</param>
        /// <param name="action">The callback action to execute when the command is invoked.</param>
        /// <returns><c>true</c> if the command was successfully added; otherwise, <c>false</c>.</returns>
        public static bool AddCommand(string commandName, Autodesk.AutoCAD.Internal.CommandCallback action)
        {
            return AddCommand(CommandGroup, commandName, CommandFlags.Modal, action);
        }

        /// <summary>
        /// Adds a new command with the specified name, command flags, and callback action using the default command group.
        /// </summary>
        /// <param name="commandName">The name of the command to add.</param>
        /// <param name="commandFlags">The command flags that define the command's behavior.</param>
        /// <param name="action">The callback action to execute when the command is invoked.</param>
        /// <returns><c>true</c> if the command was successfully added; otherwise, <c>false</c>.</returns>
        public static bool AddCommand(string commandName, CommandFlags commandFlags, Autodesk.AutoCAD.Internal.CommandCallback action)
        {
            return AddCommand(CommandGroup, commandName, commandFlags, action);
        }

        /// <summary>
        /// Adds a new command with the specified command group, name, command flags, and callback action.
        /// </summary>
        /// <param name="commandGroup">The command group to which the command belongs.</param>
        /// <param name="commandName">The name of the command to add.</param>
        /// <param name="commandFlags">The command flags that define the command's behavior.</param>
        /// <param name="action">The callback action to execute when the command is invoked.</param>
        /// <returns><c>true</c> if the command was successfully added; otherwise, <c>false</c>.</returns>
        public static bool AddCommand(string commandGroup, string commandName, CommandFlags commandFlags, Autodesk.AutoCAD.Internal.CommandCallback action)
        {
            Autodesk.AutoCAD.Internal.Utils.AddCommand(commandGroup, commandName, commandName, commandFlags, action);
            return IsCommandDefined(commandName);
        }

        /// <summary>
        /// Removes a command with the specified command group and name if it is defined.
        /// </summary>
        /// <param name="commandGroup">The command group from which to remove the command.</param>
        /// <param name="commandName">The name of the command to remove.</param>
        /// <returns><c>true</c> if the command was removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveCommand(string commandGroup, string commandName)
        {
            if (IsCommandDefined(commandName))
            {
                Autodesk.AutoCAD.Internal.Utils.RemoveCommand(commandGroup, commandName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a command with the specified name from the default command group if it is defined.
        /// </summary>
        /// <param name="commandName">The name of the command to remove.</param>
        /// <returns><c>true</c> if the command was removed; otherwise, <c>false</c>.</returns>
        public static bool RemoveCommand(string commandName)
        {
            return RemoveCommand(CommandGroup, commandName);
        }
    }
}