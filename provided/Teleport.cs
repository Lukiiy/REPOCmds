using UnityEngine;

namespace CMDS.provided;

public sealed class Teleport : Command
{
    public override string Name => "tp";
    public override string Usage => "/tp <player> <player>";

    public override void Execute(string[] args)
    {
        if (args.Length < 1) throw CommandException.Usage(this);

        PlayerAvatar target = args.Length == 1 ? PlayerAvatar.instance : Utils.Find(this, args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);

        string to = args[^1];
        PlayerAvatar dest = Utils.Find(this, to) ?? throw CommandException.PlayerNotFound(this, to);
        if (target.deadSet) throw new CommandException(this, $"{target.playerName} is dead.");

        target.Spawn(dest.transform.position + Vector3.up * .5f, target.transform.rotation);
    }
}