using System.Linq;

namespace CMDS.provided;

public sealed class Heal : Command
{
    public override string Name => "heal";
    public override string Usage => "/heal <player> [amount]";

    public override void Execute(string[] args)
    {
        PlayerAvatar avatar = Utils.Find(this, args[0]) ?? throw CommandException.PlayerNotFound(this, args[0]);
        int amount = int.TryParse(args.ElementAtOrDefault(1), out int val) && val > 0 ? val : 0;

        avatar.playerHealth.HealOther(amount == 0 ? avatar.playerHealth.maxHealth : amount, true);
        CMDS.Log.LogInfo($"Healed {avatar.playerName} by {amount}");
    }
}
