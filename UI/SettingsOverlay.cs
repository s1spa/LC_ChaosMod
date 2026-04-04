using UnityEngine;

namespace LCChaosMod.UI
{
    /// <summary>
    /// IMGUI оверлей з налаштуваннями мода.
    /// Відкривається по кліку на кнопку в settings меню гри.
    /// </summary>
    public class SettingsOverlay : MonoBehaviour
    {
        public static SettingsOverlay? Instance { get; private set; }

        private bool _visible;
        private Rect _windowRect = new(Screen.width / 2f - 300f, Screen.height / 2f - 250f, 600f, 500f);

        // Локальні значення поки не збережено
        private float _minInterval;
        private float _maxInterval;
        private int _difficulty;
        private bool _langUA;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFromConfig();
        }

        private void OnDestroy() => Instance = null;

        public void Show()
        {
            LoadFromConfig();
            _visible = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Hide() => _visible = false;

        private void OnGUI()
        {
            if (!_visible) return;

            // Темний фон
            GUI.color = new Color(0f, 0f, 0f, 0.85f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;

            _windowRect = GUI.Window(9999, _windowRect, DrawWindow, "CHAOS MOD SETTINGS");
        }

        private void DrawWindow(int id)
        {
            GUILayout.Space(10);

            // ── Мова ──────────────────────────────────────────
            GUILayout.Label("Language / Мова:");
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(!_langUA, " EN")) _langUA = false;
            if (GUILayout.Toggle(_langUA, " UA")) _langUA = true;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            // ── Таймінг ───────────────────────────────────────
            GUILayout.Label($"Min interval: {_minInterval:F0}s");
            _minInterval = GUILayout.HorizontalSlider(_minInterval, 30f, 300f);

            GUILayout.Label($"Max interval: {_maxInterval:F0}s");
            _maxInterval = GUILayout.HorizontalSlider(_maxInterval, 60f, 600f);

            GUILayout.Space(10);

            // ── Складність ────────────────────────────────────
            GUILayout.Label($"Difficulty: {DifficultyLabel(_difficulty)}");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1 - Easy")) _difficulty = 1;
            if (GUILayout.Button("2 - Normal")) _difficulty = 2;
            if (GUILayout.Button("3 - Chaos")) _difficulty = 3;
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            // ── Евенти ────────────────────────────────────────
            GUILayout.Label("Events:");
            ChaosSettings.EnableMines.Value = GUILayout.Toggle(ChaosSettings.EnableMines.Value, " Mines");
            ChaosSettings.EnableTurrets.Value = GUILayout.Toggle(ChaosSettings.EnableTurrets.Value, " Turrets");
            ChaosSettings.EnableMobSpawn.Value = GUILayout.Toggle(ChaosSettings.EnableMobSpawn.Value, " Random mob");
            ChaosSettings.EnableTeleportDungeon.Value = GUILayout.Toggle(ChaosSettings.EnableTeleportDungeon.Value, " Teleport (dungeon)");
            ChaosSettings.EnableTeleportShip.Value = GUILayout.Toggle(ChaosSettings.EnableTeleportShip.Value, " Teleport to ship");
            ChaosSettings.EnablePlayerSwap.Value = GUILayout.Toggle(ChaosSettings.EnablePlayerSwap.Value, " Player swap");
            ChaosSettings.EnableGlowstick.Value = GUILayout.Toggle(ChaosSettings.EnableGlowstick.Value, " Glowstick (Apparatus)");

            GUILayout.Space(15);

            // ── Кнопки ────────────────────────────────────────
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save & Close"))
            {
                SaveToConfig();
                Hide();
            }
            if (GUILayout.Button("Cancel"))
            {
                Hide();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        private void LoadFromConfig()
        {
            _minInterval = ChaosSettings.MinInterval.Value;
            _maxInterval = ChaosSettings.MaxInterval.Value;
            _difficulty = ChaosSettings.Difficulty.Value;
            _langUA = ChaosSettings.Language.Value == "UA";
        }

        private void SaveToConfig()
        {
            ChaosSettings.MinInterval.Value = _minInterval;
            ChaosSettings.MaxInterval.Value = _maxInterval;
            ChaosSettings.Difficulty.Value = _difficulty;
            ChaosSettings.Language.Value = _langUA ? "UA" : "EN";
        }

        private static string DifficultyLabel(int d) => d switch
        {
            1 => "Easy",
            3 => "Chaos",
            _ => "Normal"
        };
    }
}
