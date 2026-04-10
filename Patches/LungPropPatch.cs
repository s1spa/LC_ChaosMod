using LCChaosMod.Cogs.Firefly;
using UnityEngine;

namespace LCChaosMod.Patches
{
    // Кожен кадр перевіряє чи локальний гравець тримає Apparatus (LungProp).
    // При підніманні — вмикає Firefly glow один раз.
    public class LungPropWatcher : MonoBehaviour
    {
        private bool _triggered;

        private void Update()
        {
            // Раунд закінчився — прибираємо glow і скидаємо
            if (StartOfRound.Instance != null && StartOfRound.Instance.inShipPhase)
            {
                if (_triggered)
                {
                    _triggered = false;
                    FireflyTracker.Cleanup();
                }
                return;
            }

            if (_triggered) return;
            if (!ChaosSettings.EnableGlowstick.Value) return;

            var player = GameNetworkManager.Instance?.localPlayerController;
            if (player == null) return;
            if (player.currentlyHeldObjectServer is not LungProp) return;

            _triggered = true;
            Plugin.Log.LogInfo("[LungPropWatcher] Local player grabbed Apparatus.");
            FireflyTracker.OnLocalPlayerGrabbed();
        }
    }
}
