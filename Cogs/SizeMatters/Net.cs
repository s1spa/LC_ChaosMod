using System.Collections;
using GameNetcodeStuff;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace LCChaosMod.Cogs.SizeMatters
{
    internal static class Net
    {
        private const string MsgShrink = "LCChaosMod_Shrink";

        public static void Init()
        {
            NetworkManager.Singleton.CustomMessagingManager
                .RegisterNamedMessageHandler(MsgShrink, OnReceive);
        }

        /// <summary>Called by host: broadcasts shrink to all clients.</summary>
        public static void Broadcast(ulong targetClientId, float scale, float duration)
        {
            ApplyShrink(targetClientId, scale, duration);

            var writer = new FastBufferWriter(20, Allocator.Temp);
            using (writer)
            {
                writer.WriteValueSafe(targetClientId);
                writer.WriteValueSafe(scale);
                writer.WriteValueSafe(duration);
                NetworkManager.Singleton.CustomMessagingManager
                    .SendNamedMessageToAll(MsgShrink, writer);
            }
        }

        private static void OnReceive(ulong _, FastBufferReader reader)
        {
            if (NetworkManager.Singleton.IsServer) return;
            reader.ReadValueSafe(out ulong clientId);
            reader.ReadValueSafe(out float scale);
            reader.ReadValueSafe(out float duration);
            ApplyShrink(clientId, scale, duration);
        }

        private static void ApplyShrink(ulong clientId, float scale, float duration)
        {
            var player = FindPlayer(clientId);
            if (player == null)
            {
                Plugin.Log.LogWarning($"[SizeMatters] Player {clientId} not found.");
                return;
            }
            GameNetworkManager.Instance.StartCoroutine(ShrinkCoroutine(player, scale, duration));
        }

        private static IEnumerator ShrinkCoroutine(PlayerControllerB player, float scale, float duration)
        {
            Plugin.Log.LogInfo($"[SizeMatters] {player.playerUsername} → scale {scale} for {duration}s.");

            player.thisPlayerBody.localScale = new Vector3(scale, scale, scale);

            // Move camera down for the local player only
            bool isLocal = player == GameNetworkManager.Instance?.localPlayerController;
            Transform? cam = isLocal ? player.gameplayCamera?.transform : null;
            Vector3 origCamPos = cam != null ? cam.localPosition : Vector3.zero;
            if (cam != null)
                cam.localPosition = new Vector3(origCamPos.x, origCamPos.y * scale, origCamPos.z);

            int pidx = (int)player.playerClientId;
            float elapsed = 0f;

            while (elapsed < duration && player != null)
            {
                // SoundManager built-in pitch system
                if (SoundManager.Instance != null && pidx < SoundManager.Instance.playerVoicePitchTargets.Length)
                    SoundManager.Instance.playerVoicePitchTargets[pidx] = 2f;

                // Also set directly on AudioSource (Dissonance may reassign it each frame)
                if (player.currentVoiceChatAudioSource != null)
                    player.currentVoiceChatAudioSource.pitch = 2f;

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (player != null)
                player.thisPlayerBody.localScale = Vector3.one;
            if (cam != null)
                cam.localPosition = origCamPos;
            if (SoundManager.Instance != null && pidx < SoundManager.Instance.playerVoicePitchTargets.Length)
                SoundManager.Instance.playerVoicePitchTargets[pidx] = 1f;
            if (player != null && player.currentVoiceChatAudioSource != null)
                player.currentVoiceChatAudioSource.pitch = 1f;

            Plugin.Log.LogInfo($"[SizeMatters] {player.playerUsername} restored.");
        }

        private static PlayerControllerB? FindPlayer(ulong clientId)
        {
            foreach (var p in StartOfRound.Instance.allPlayerScripts)
                if (p.actualClientId == clientId && p.isPlayerControlled && !p.isPlayerDead)
                    return p;
            return null;
        }
    }
}
