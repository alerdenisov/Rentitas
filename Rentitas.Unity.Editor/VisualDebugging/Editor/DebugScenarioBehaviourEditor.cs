using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rentitas.Unity.VisualDebugging
{
    [CustomEditor(typeof(DebugScenarioBehaviour))]
    public class DebugScenarioBehaviourEditor : Editor {

        enum SortMethod
        {
            OrderOfOccurrence,

            Name,
            NameDescending,

            ExecutionTime,
            ExecutionTimeDescending
        }

        SystemsMonitor _systemsMonitor;
        Queue<float> _systemMonitorData;
        const int SYSTEM_MONITOR_DATA_LENGTH = 60;

        static bool _showInitializeSystems = true;
        static bool _showExecuteSystems = true;
        static bool _hideEmptySystems = true;
        static string _systemNameSearchTerm = string.Empty;

        float _threshold;
        SortMethod _systemSortMethod;

        int _lastRenderedFrameCount;

        public override void OnInspectorGUI()
        {
            var debugBeh = (DebugScenarioBehaviour) target;
            var scenario = debugBeh.Scenario;

            DrawSystemsOverview(scenario);
            DrawSystemMonitor(scenario);
            DrawSystemList(scenario);

            EditorUtility.SetDirty(target);
        }

        private void DrawSystemsOverview(DebugScenario scenario)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField(scenario.Name, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Initialize Systems", scenario.InitializeSystemsCount.ToString());
                EditorGUILayout.LabelField("Execute Systems", scenario.ExecuteSystemsCount.ToString());
                EditorGUILayout.LabelField("Total Systems", scenario.TotalSystemsCount.ToString());
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSystemMonitor(DebugScenario scenario)
        {
            if (_systemsMonitor == null)
            {
                _systemsMonitor = new SystemsMonitor(SYSTEM_MONITOR_DATA_LENGTH);
                _systemMonitorData = new Queue<float>(new float[SYSTEM_MONITOR_DATA_LENGTH]);
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Execution duration", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Total", scenario.TotalDuration.ToString());

                    var buttonStyle = new GUIStyle(GUI.skin.button);
                    if (scenario.IsPaused)
                    {
                        buttonStyle.normal = GUI.skin.button.active;
                    }
                    if (GUILayout.Button("▌▌", buttonStyle, GUILayout.Width(50)))
                    {
                        scenario.IsPaused = !scenario.IsPaused;
                    }
                    if (GUILayout.Button("Step", GUILayout.Width(50)))
                    {
                        scenario.IsPaused = true;
                        scenario.Step();
                        AddDuration((float) scenario.TotalDuration);
                        _systemsMonitor.Draw(_systemMonitorData.ToArray(), 80f);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (!EditorApplication.isPaused && !scenario.IsPaused)
                {
                    AddDuration((float) scenario.TotalDuration);
                }
                _systemsMonitor.Draw(_systemMonitorData.ToArray(), 80f);
            }
            EditorGUILayout.EndVertical();
        }


        private void DrawSystemList(DebugScenario systems)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    DebugScenario.AvgResetInterval = (AvgResetInterval)EditorGUILayout.EnumPopup("Reset average duration Ø", DebugScenario.AvgResetInterval);
                    if (GUILayout.Button("Reset Ø now", GUILayout.Width(88), GUILayout.Height(14)))
                    {
                        systems.ResetDurations();
                    }
                }
                EditorGUILayout.EndHorizontal();

                _threshold = EditorGUILayout.Slider("Threshold Ø ms", _threshold, 0f, 33f);
                _systemSortMethod = (SortMethod)EditorGUILayout.EnumPopup("Sort by ", _systemSortMethod);
                _hideEmptySystems = EditorGUILayout.Toggle("Hide empty systems", _hideEmptySystems);
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    _systemNameSearchTerm = EditorGUILayout.TextField("Search", _systemNameSearchTerm);

                    const string clearButtonControlName = "Clear Button";
                    GUI.SetNextControlName(clearButtonControlName);
                    if (GUILayout.Button("x", GUILayout.Width(19), GUILayout.Height(14)))
                    {
                        _systemNameSearchTerm = string.Empty;
                        GUI.FocusControl(clearButtonControlName);
                    }
                }
                EditorGUILayout.EndHorizontal();

                _showInitializeSystems = EditorGUILayout.Foldout(_showInitializeSystems, "Initialize Systems");
                if (_showInitializeSystems && ShouldShowSystems(systems, true))
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        var systemsDrawn = DrawSystemInfos(systems, true, false);
                        if (systemsDrawn == 0)
                        {
                            EditorGUILayout.LabelField(string.Empty);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }

                _showExecuteSystems = EditorGUILayout.Foldout(_showExecuteSystems, "Execute Systems");
                if (_showExecuteSystems && ShouldShowSystems(systems, false))
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        var systemsDrawn = DrawSystemInfos(systems, false, false);
                        if (systemsDrawn == 0)
                        {
                            EditorGUILayout.LabelField(string.Empty);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private int DrawSystemInfos(DebugScenario systems, bool initOnly, bool isChildSystem)
        {
            var systemInfos = initOnly ? systems.InitializeSystemInfos : systems.ExecuteSystemInfos;
            systemInfos = systemInfos
                .Where(systemInfo => systemInfo.averageExecutionDuration >= _threshold)
                .ToArray();

            systemInfos = GetSortedSystemInfos(systemInfos, _systemSortMethod);

            var systemsDrawn = 0;
            foreach (var systemInfo in systemInfos)
            {
                var debugSystems = systemInfo.system as DebugScenario;
                if (debugSystems != null)
                {
                    if (!ShouldShowSystems(debugSystems, initOnly))
                    {
                        continue;
                    }
                }

                if (systemInfo.systemName.ToLower().Contains(_systemNameSearchTerm.ToLower()))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUI.BeginDisabledGroup(isChildSystem);
                        {
                            systemInfo.isActive = EditorGUILayout.Toggle(systemInfo.isActive, GUILayout.Width(20));
                        }
                        EditorGUI.EndDisabledGroup();
                        var reactiveSystem = systemInfo.system as IReactiveInternalSystem;
                        if (reactiveSystem != null)
                        {
                            if (systemInfo.isActive)
                            {
                                reactiveSystem.Activate();
                            }
                            else
                            {
                                reactiveSystem.Deactivate();
                            }
                        }

                        var avg = string.Format("Ø {0:00.000}", systemInfo.averageExecutionDuration).PadRight(12);
                        var min = string.Format("▼ {0:00.000}", systemInfo.minExecutionDuration).PadRight(12);
                        var max = string.Format("▲ {0:00.000}", systemInfo.maxExecutionDuration);

                        EditorGUILayout.LabelField(systemInfo.systemName, avg + min + max, GetSystemStyle(systemInfo));
                    }
                    EditorGUILayout.EndHorizontal();

                    systemsDrawn += 1;
                }

                var debugSystem = systemInfo.system as DebugScenario;
                if (debugSystem != null)
                {
                    var indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel += 1;
                    systemsDrawn += DrawSystemInfos(debugSystem, initOnly, true);
                    EditorGUI.indentLevel = indent;
                }
            }

            return systemsDrawn;
        }

        static GUIStyle GetSystemStyle(SystemInfo systemInfo)
        {
            var style = new GUIStyle(GUI.skin.label);
            var color = systemInfo.isReactiveSystems && EditorGUIUtility.isProSkin
                            ? Color.white
                            : style.normal.textColor;

            style.normal.textColor = color;

            return style;
        }
        private bool ShouldShowSystems(DebugScenario systems, bool initOnly)
        {
            if (!_hideEmptySystems)
            {
                return true;
            }

            if (initOnly)
            {
                return systems.TotalInitializeSystemsCount > 0;
            }

            return systems.TotalExecuteSystemsCount > 0;
        }

        static SystemInfo[] GetSortedSystemInfos(SystemInfo[] systemInfos, SortMethod sortMethod)
        {
            if (sortMethod == SortMethod.Name)
            {
                return systemInfos
                    .OrderBy(systemInfo => systemInfo.systemName)
                    .ToArray();
            }
            if (sortMethod == SortMethod.NameDescending)
            {
                return systemInfos
                    .OrderByDescending(systemInfo => systemInfo.systemName)
                    .ToArray();
            }

            if (sortMethod == SortMethod.ExecutionTime)
            {
                return systemInfos
                    .OrderBy(systemInfo => systemInfo.averageExecutionDuration)
                    .ToArray();
            }
            if (sortMethod == SortMethod.ExecutionTimeDescending)
            {
                return systemInfos
                    .OrderByDescending(systemInfo => systemInfo.averageExecutionDuration)
                    .ToArray();
            }

            return systemInfos;
        }

        private void AddDuration(float duration)
        {
            // OnInspectorGUI is called twice per frame - only add duration once
            if (Time.renderedFrameCount != _lastRenderedFrameCount)
            {
                _lastRenderedFrameCount = Time.renderedFrameCount;

                if (_systemMonitorData.Count >= SYSTEM_MONITOR_DATA_LENGTH)
                {
                    _systemMonitorData.Dequeue();
                }

                _systemMonitorData.Enqueue(duration);
            }
        }
    }
}
