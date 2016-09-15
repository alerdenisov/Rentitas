using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Rentitas.Unity.VisualDebugging
{
    public enum AvgResetInterval
    {
        Always = 1,
        VeryFast = 30,
        Fast = 60,
        Normal = 120,
        Slow = 300,
        Never = int.MaxValue
    }

    public class DebugScenario : BaseScenario
    {
        public static AvgResetInterval AvgResetInterval = AvgResetInterval.Never;

        public GameObject Container => _container.gameObject;
        public string Name => _name;
        public bool IsPaused => _paused;

        readonly List<ISystem> _systems;
        readonly Transform _container;
        readonly List<SystemInfo> _initializeSystemInfos;
        readonly List<SystemInfo> _executeSystemInfos;
        readonly Stopwatch _stopwatch;
        readonly string _name;
        double _totalDuration;
        bool _paused;

        public DebugScenario(string name = "Systems") : base()
        {
            _name = name;
            _systems = new List<ISystem>();
            _container = new GameObject().transform;
            _container.gameObject.AddComponent<DebugScenarioBehaviour>().Init(this);
            _initializeSystemInfos = new List<SystemInfo>();
            _executeSystemInfos = new List<SystemInfo>();
            _stopwatch = new Stopwatch();
            updateName();
        }

        public override BaseScenario Add(ISystem system)
        {
            _systems.Add(system);
            var debugSystems = system as DebugScenario;
            debugSystems?.Container.transform.SetParent(_container.transform, false);

            var systemInfo = new SystemInfo(system);

            if (systemInfo.isInitializeSystems)
                _initializeSystemInfos.Add(systemInfo);

            if (systemInfo.isExecuteSystems || systemInfo.isReactiveSystems)
                _executeSystemInfos.Add(systemInfo);

            return base.Add(system);
        }

        public override void Initialize()
        {
            _totalDuration = 0;
            for (int i = 0; i < InitializeSystems.Count; i++)
            {
                var system = InitializeSystems[i];
                var systemInfo = _initializeSystemInfos[i];
                if (systemInfo.isActive)
                {
                    var duration = monitorSystemInitializeDuration(system);
                    _totalDuration += duration;
                    systemInfo.AddExecutionDuration(duration);
                }
            }

            updateName();
        }

        public override void Execute()
        {
            if (!IsPaused)
            {
                Step();
            }
        }

        public void Step()
        {
            _totalDuration = 0;
            if (Time.frameCount % (int)AvgResetInterval == 0)
            {
                ResetDurations();
            }
            for (int i = 0; i < ExecuteSystems.Count; i++)
            {
                var system = ExecuteSystems[i];
                var systemInfo = _executeSystemInfos[i];
                if (systemInfo.isActive)
                {
                    var duration = monitorSystemExecutionDuration(system);
                    _totalDuration += duration;
                    systemInfo.AddExecutionDuration(duration);
                }
            }

            updateName();
        }

        void updateName()
        {
            if (_container != null)
            {
                _container.name = string.Format("{0} ({1} init, {2} exe, {3:0.###} ms)",
                    _name, InitializeSystems.Count, ExecuteSystems.Count, _totalDuration);
            }
        }

        double monitorSystemInitializeDuration(IInitializeSystem system)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            system.Initialize();
            _stopwatch.Stop();
            return _stopwatch.Elapsed.TotalMilliseconds;
        }

        double monitorSystemExecutionDuration(IExecuteSystem system)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            system.Execute();
            _stopwatch.Stop();
            return _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void ResetDurations()
        {
            foreach (var systemInfo in _initializeSystemInfos)
            {
                systemInfo.ResetDurations();
            }
            foreach (var systemInfo in _executeSystemInfos)
            {
                systemInfo.ResetDurations();
                var debugSystems = systemInfo.system as DebugScenario;
                debugSystems?.ResetDurations();
            }
        }
    }
}