namespace LCChaosMod.Cogs
{
    public class InfiniteStaminaEvent : IChaosEvent
    {
        public string GetName()   => Loc.Get("event.infinite_stamina");
        public bool   IsEnabled() => ChaosSettings.EnableInfiniteStamina.Value;

        public void Execute()
        {
            ChaosNetworkHandler.BroadcastStamina(ChaosSettings.StaminaDuration.Value);
        }
    }
}
