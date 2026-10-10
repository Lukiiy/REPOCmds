using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace CMDS;

public static class Utils
{
    public static PlayerAvatar Find(Command cmd, string name)
    {
        if (name.Equals("me", StringComparison.OrdinalIgnoreCase)) return PlayerAvatar.instance;

        var players = GameDirector.instance.PlayerList.Where(p => p).ToList();

        var exact = players.FirstOrDefault(p => string.Equals(p.playerName, name, StringComparison.OrdinalIgnoreCase));
        if (exact) return exact;

        var prefix = players.Where(p => p.playerName.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (prefix.Count == 1) return prefix[0];

        var contains = players.Where(p => p.playerName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        if (contains.Count == 1) return contains[0];

        if (prefix.Count + contains.Count > 1) throw new CommandException(cmd, $"\"{name}\" matches more than one player.");
        throw CommandException.PlayerNotFound(cmd, name);
    }

    public static GameObject SpawnPrefab(PrefabRef prefab, Vector3 pos, Quaternion rot) => GameManager.instance.gameMode == 0 ? UnityEngine.Object.Instantiate(prefab.Prefab, pos, rot) : PhotonNetwork.InstantiateRoomObject(prefab.ResourcePath, pos, rot, 0);
}