using LCChaosMod.Utils;
using Unity.Netcode;
using UnityEngine;

namespace LCChaosMod.Cogs
{
    public class FootballEvent : IChaosEvent
    {
        public string GetName()   => Loc.Get("event.football");
        public bool   IsEnabled() => ChaosSettings.EnableFootball.Value;

        public void Execute()
        {
            if (!NetworkManager.Singleton.IsServer)
            {
                Plugin.Log.LogInfo("[FootballEvent] Skipped - not host.");
                return;
            }

            float duration = ChaosSettings.FootballDuration.Value;
            Plugin.Log.LogInfo($"[FootballEvent] Starting for {duration}s.");

            var go = new GameObject("FootballWatcher");
            var watcher = go.AddComponent<Football.FootballWatcher>();
            watcher.Setup(duration);
        }
    }
}
