using System;
using System.Collections;
using System.Collections.Generic;
using LCChaosMod.Cogs;
using UnityEngine;

namespace LCChaosMod
{
    /// <summary>
    /// Запускається на початку раунду. Кожні N секунд обирає і виконує рандомний евент.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public static EventManager? Instance { get; private set; }

        private readonly List<IChaosEvent> _events = new();
        private Coroutine? _loop;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            // TODO: зареєструвати евенти тут коли будуть готові
            // _events.Add(new MineSpawnEvent());

            if (_events.Count == 0)
            {
                Plugin.Log.LogWarning("EventManager: жодного евенту не зареєстровано.");
                return;
            }

            _loop = StartCoroutine(EventLoop());
        }

        private void OnDestroy()
        {
            if (_loop != null) StopCoroutine(_loop);
            Instance = null;
        }

        private IEnumerator EventLoop()
        {
            while (true)
            {
                float wait = UnityEngine.Random.Range(ChaosSettings.MinInterval.Value, ChaosSettings.MaxInterval.Value);

                // Чекаємо (інтервал мінус 5 секунд для попередження)
                yield return new WaitForSeconds(Mathf.Max(0f, wait - 5f));

                var next = PickEvent();
                if (next == null) { yield return new WaitForSeconds(5f); continue; }

                ShowWarning(next.GetName());
                yield return new WaitForSeconds(5f);

                try { next.Execute(); }
                catch (Exception ex) { Plugin.Log.LogError($"[EventManager] Помилка '{next.GetName()}': {ex}"); }
            }
        }

        private IChaosEvent? PickEvent()
        {
            var available = _events.FindAll(e => e.IsEnabled());
            if (available.Count == 0) return null;
            return available[UnityEngine.Random.Range(0, available.Count)];
        }

        private static void ShowWarning(string eventName)
        {
            bool ua = ChaosSettings.Language.Value == "UA";
            string title = ua ? "ХАОС" : "CHAOS";
            string msg = ua
                ? $"{eventName} через 5 секунд!"
                : $"{eventName} in 5 seconds!";

            HUDManager.Instance?.DisplayTip(title, msg, isWarning: true);
        }
    }
}
