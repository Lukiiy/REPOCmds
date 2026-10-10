using System.Linq;
using System.Text.RegularExpressions;

namespace CMDS.provided;

public sealed class Dump : Command
{
    public override string Name => "dump";
    public override string Usage => "/dump";

    public override void Execute(string[] args)
    {
        EnemyDirector ed = EnemyDirector.instance;
        var items = StatsManager.instance.itemDictionary.Values.Where(i => i).Select(i => Regex.Replace(i.name, "^Item ", "")).OrderBy(n => n);
        var enemies = ed.enemiesDifficulty1.Concat(ed.enemiesDifficulty2).Concat(ed.enemiesDifficulty3).Where(e => e).Select(e => Regex.Replace(e.name, "^Enemy (- )?", "")).Distinct().OrderBy(n => n);

        CMDS.Log.LogInfo($"Items: {string.Join(", ", items)}");
        CMDS.Log.LogInfo($"Enemies: {string.Join(", ", enemies)}");
    }
}