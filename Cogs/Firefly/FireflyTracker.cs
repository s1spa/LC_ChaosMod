using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;

namespace LCChaosMod.Cogs.Firefly
{
    public class FireflyLight : MonoBehaviour { }

    internal static class FireflyTracker
    {
        private static readonly Color GlowColor = new Color(1f, 0.85f, 0.4f); // теплий жовтий
        private const float Range     = 8f;
        private const float Intensity = 3f;

        private static readonly List<FireflyLight> _active = new();

        public static void OnLocalPlayerGrabbed()
        {
            var local = GameNetworkManager.Instance?.localPlayerController;
            if (local == null) return;

            AddLight(local);
            Net.Broadcast(local.actualClientId);
        }

        public static void AddLightToPlayer(ulong clientId)
        {
            var all = StartOfRound.Instance?.allPlayerScripts;
            if (all == null) return;

            foreach (var p in all)
            {
                if (p.actualClientId == clientId)
                {
                    AddLight(p);
                    return;
                }
            }
        }

        private static void AddLight(PlayerControllerB player)
        {
            // ! Не дублюємо
            if (player.GetComponentInChildren<FireflyLight>() != null) return;

            var go = new GameObject("FireflyLight");
            go.transform.SetParent(player.transform, false);
            go.transform.localPosition = new Vector3(0f, 1f, 0f);

            var light      = go.AddComponent<Light>();
            light.type      = LightType.Point;
            light.color     = GlowColor;
            light.range     = Range;
            light.intensity = Intensity;
            light.shadows   = LightShadows.None;

            var marker = go.AddComponent<FireflyLight>();
            _active.Add(marker);
            Plugin.Log.LogInfo($"[Firefly] Glow added to {player.playerUsername}.");
        }

        // Видалити всі firefly вогники в кінці раунду.
        public static void Cleanup()
{
    _active.Clear(); // Чистимо список

    var allPlayers = StartOfRound.Instance?.allPlayerScripts;
    if (allPlayers == null) return;

    foreach (var p in allPlayers)
    {
        // Шукаємо всі об'єкти з нашим скриптом-маркером на гравцеві
        var lights = p.GetComponentsInChildren<FireflyLight>();
        foreach (var l in lights)
        {
            Object.Destroy(l.gameObject);
        }
    }
    Plugin.Log.LogInfo("[Firefly] Deep cleanup finished.");
}
    }
}
