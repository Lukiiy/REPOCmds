using System;
using System.Collections.Generic;
using System.Linq;

namespace CMDS;

public static class CommandAPI
{
    private static readonly Dictionary<string, Command> commands = [];

    public static void Register(Command command) => commands[command.Name.ToLower()] = command;

    public static bool TryHandle(string message)
    {
        if (string.IsNullOrEmpty(message) || !message.StartsWith('/')) return false;

        string[] parts = message[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return false;

        if (!commands.TryGetValue(parts[0].ToLower(), out Command command)) return false;

        try
        {
            command.Execute([.. parts.Skip(1)]);
        }
        catch (Exception e)
        {
            CMDS.Log.LogError($"An error occurred while executing \"/{command.Name}\". {e.Message}");
        }

        return true;
    }
}