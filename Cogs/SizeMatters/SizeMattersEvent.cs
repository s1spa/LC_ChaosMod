using System.Collections.Generic;
using LCChaosMod.Utils;
using Unity.Netcode;
using UnityEngine;

namespace LCChaosMod.Cogs.SizeMatters
{
    public class SizeMattersEvent : IChaosEvent
    {
        public string GetName()   => Loc.Get("event.size_matters");
        public bool   IsEnabled() => ChaosSettings.EnableSizeMatters.Value;

        public void Execute()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                Plugin.Log.LogInfo("[SizeMattersEvent] Skipped - not host.");
                return;
            }

            var eligible = new List<GameNetcodeStuff.PlayerControllerB>();
            foreach (var p in StartOfRound.Instance.allPlayerScripts)
                if (p.isPlayerControlled && !p.isPlayerDead && !p.isInHangarShipRoom)
                    eligible.Add(p);

            if (eligible.Count == 0)
            {
                Plugin.Log.LogInfo("[SizeMattersEvent] No eligible players.");
                return;
            }

            var target   = eligible[Random.Range(0, eligible.Count)];
            float scale  = ChaosSettings.SizeScale.Value;
            float dur    = ChaosSettings.SizeDuration.Value;

            Plugin.Log.LogInfo($"[SizeMattersEvent] Shrinking {target.playerUsername} to {scale} for {dur}s.");
            Net.Broadcast(target.actualClientId, scale, dur);
        }
    }
}
