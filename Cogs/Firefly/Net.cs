using Unity.Collections;
using Unity.Netcode;

namespace LCChaosMod.Cogs.Firefly
{
    internal static class Net
    {
        private const string MsgFirefly = "LCChaosMod_Firefly";

        public static void Init()
        {
            NetworkManager.Singleton.CustomMessagingManager
                .RegisterNamedMessageHandler(MsgFirefly, OnReceive);
        }

        /// <summary>Broadcast that this client now has the firefly glow.</summary>
        public static void Broadcast(ulong clientId)
        {
            var writer = new FastBufferWriter(8, Allocator.Temp);
            using (writer)
            {
                writer.WriteValueSafe(clientId);
                NetworkManager.Singleton.CustomMessagingManager
                    .SendNamedMessageToAll(MsgFirefly, writer);
            }
        }

        private static void OnReceive(ulong _, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ulong clientId);

            // Skip if this is the local player — already applied locally
            if (clientId == NetworkManager.Singleton.LocalClientId) return;

            FireflyTracker.AddLightToPlayer(clientId);
        }
    }
}
