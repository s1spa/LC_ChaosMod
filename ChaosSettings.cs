using BepInEx.Configuration;

namespace LCChaosMod
{
    public static class ChaosSettings
    {
        // Загальні
        public static ConfigEntry<bool> ModEnabled { get; private set; } = null!;
        public static ConfigEntry<string> Language { get; private set; } = null!;

        // Таймінги
        public static ConfigEntry<float> MinInterval { get; private set; } = null!;
        public static ConfigEntry<float> MaxInterval { get; private set; } = null!;

        // Складність (впливає на кількість/силу евентів)
        public static ConfigEntry<int> Difficulty { get; private set; } = null!;

        // Які евенти увімкнені
        public static ConfigEntry<bool> EnableMines { get; private set; } = null!;
        public static ConfigEntry<bool> EnableTurrets { get; private set; } = null!;
        public static ConfigEntry<bool> EnableMobSpawn { get; private set; } = null!;
        public static ConfigEntry<bool> EnableTeleportDungeon { get; private set; } = null!;
        public static ConfigEntry<bool> EnableTeleportShip { get; private set; } = null!;
        public static ConfigEntry<bool> EnablePlayerSwap { get; private set; } = null!;
        public static ConfigEntry<bool> EnableGlowstick { get; private set; } = null!;

        public static void Init(ConfigFile config)
        {
            ModEnabled = config.Bind("General", "Enabled", true, "Вмикає/вимикає мод повністю");
            Language = config.Bind("General", "Language", "EN", "Мова повідомлень: EN / UA");

            MinInterval = config.Bind("Timing", "MinInterval", 120f, "Мінімальний інтервал між евентами (секунди)");
            MaxInterval = config.Bind("Timing", "MaxInterval", 300f, "Максимальний інтервал між евентами (секунди)");

            Difficulty = config.Bind("Difficulty", "Level", 2, "Складність: 1 (легко) — 3 (хаос)");

            EnableMines = config.Bind("Events", "Mines", true, "Міни під ногами");
            EnableTurrets = config.Bind("Events", "Turrets", true, "Турелі навколо");
            EnableMobSpawn = config.Bind("Events", "MobSpawn", true, "Спавн рандомного моба");
            EnableTeleportDungeon = config.Bind("Events", "TeleportDungeon", true, "Рандомна телепортація по данжу");
            EnableTeleportShip = config.Bind("Events", "TeleportShip", true, "Телепортація на корабель");
            EnablePlayerSwap = config.Bind("Events", "PlayerSwap", true, "Підміна гравців місцями");
            EnableGlowstick = config.Bind("Events", "Glowstick", true, "Світіння при підйомі Apparatus");
        }
    }
}
