namespace CMDS.provided;

public sealed class Revive : Command
{
    public override string Name => "revive";
    public override string Description => "Revive a player.";
    public override string Usage => "/revive [player]";

    public override void Execute(string[] args)
    {
        string target = string.Join(' ', args).Trim();

        PlayerAvatar player = target.Length == 0 ? PlayerAvatar.instance : Utils.Find(this, target);
        if (!player) throw CommandException.PlayerNotFound(this, target);
        if (!player.deadSet || !player.playerDeathHead) throw new CommandException(this, $"{player.playerName} is not dead.");

        player.playerDeathHead.inExtractionPoint = true;
        player.playerDeathHead.Revive();
        CMDS.Log.LogInfo($"Revived {player.playerName}.");
    }
}
