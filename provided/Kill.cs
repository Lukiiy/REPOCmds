namespace CMDS.provided;

public sealed class Kill : Command
{
    public override string Name => "kill";
    public override string Usage => "/kill [player]";

    public override void Execute(string[] args)
    {
        string target = string.Join(' ', args).Trim();

        PlayerAvatar player = target.Length == 0 ? PlayerAvatar.instance : Utils.Find(this, target);
        if (!player) throw CommandException.PlayerNotFound(this, target);

        player.PlayerDeath(-1);
        CMDS.Log.LogInfo($"Killed {player.playerName}.");
    }
}
