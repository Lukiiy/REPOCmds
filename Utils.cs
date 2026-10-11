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
        List<PlayerAvatar> players = GameDirector.instance.PlayerList.Where(p => p).ToList();

        var exact = players.FirstOrDefault(p => string.Equals(p.playerName, name, StringComparison.OrdinalIgnoreCase));
        if (exact) return exact;

        var prefix = players.Where(p => p.playerName.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (prefix.Count > 0) return prefix[0];

        throw CommandException.PlayerNotFound(cmd, name);
    }

    public static GameObject SpawnPrefab(PrefabRef prefab, Vector3 pos, Quaternion rot) => GameManager.instance.gameMode == 0 ? UnityEngine.Object.Instantiate(prefab.Prefab, pos, rot) : PhotonNetwork.InstantiateRoomObject(prefab.ResourcePath, pos, rot, 0);
}