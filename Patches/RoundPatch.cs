using UnityEngine;
using UnityEngine.SceneManagement;

namespace LCChaosMod.Patches
{
    // Запускає/зупиняє EventManager при зміні сцен.
    // * Використовує SceneManager.sceneLoaded — так само як MainMenuInjector.
    // Level* — ігровий раунд. SampleSceneRelay — лобі/корабель.
    internal static class RoundLifecycle
    {
        public static void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Plugin.Log.LogInfo("[RoundLifecycle] Registered sceneLoaded.");
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Plugin.Log.LogInfo($"[RoundLifecycle] Scene loaded: '{scene.name}'");

            if (scene.name.StartsWith("Level"))
            {
                ChaosNetworkHandler.Init();
                StartRound();
            }
            else if (scene.name == "SampleSceneRelay")
            {
                StopRound();
            }
        }

        private static void StartRound()
        {
            // * Watcher запускається на всіх клієнтах, не тільки на хості
            var watcher = new GameObject("LungPropWatcher");
            watcher.AddComponent<LungPropWatcher>();

            if (!ChaosSettings.ModEnabled.Value) return;
            if (!Unity.Netcode.NetworkManager.Singleton.IsServer) return;
            if (EventManager.Instance != null) return;
            // ! Пропускаємо Gordion (місяць компанії)
            if (StartOfRound.Instance != null && StartOfRound.Instance.currentLevelID == 3) return;

            var go = new GameObject("ChaosEventManager");
            go.AddComponent<EventManager>();
            Plugin.Log.LogInfo("[RoundLifecycle] EventManager started.");
        }

        private static void StopRound()
        {
            if (EventManager.Instance != null)
            {
                Object.Destroy(EventManager.Instance.gameObject);
                Plugin.Log.LogInfo("[RoundLifecycle] EventManager stopped.");
            }

            Cogs.Firefly.FireflyTracker.Cleanup();
        }
    }
}
