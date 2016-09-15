using UnityEngine;

namespace Rentitas.Unity
{
    public class DrawExecutorWidget : IKernelExplorerWidget
    {
        private DrawPoolTypesInAssemblyWidget _drawPoolTypes;
        private DrawComponentTypesInSelectedPool _drawComponentTypes;
        private DrawComponentFieldsWidget _drawComponentFields;
        private ExplorerConfigWidget _drawExplorerConfig;

        private Vector2 _scrollPoolTypes;
        private Vector2 _scrollComponentTypes;

        public DrawExecutorWidget()
        {
            _drawPoolTypes = new DrawPoolTypesInAssemblyWidget();
            _drawComponentTypes = new DrawComponentTypesInSelectedPool();
            _drawComponentFields = new DrawComponentFieldsWidget();
            _drawExplorerConfig = new ExplorerConfigWidget();
        }

        public void DrawWidget(KernelExplorer explorer)
        {
            if (!explorer.Executor.IsNotConfigurated)
            {
                _drawExplorerConfig.DrawWidget(explorer);
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    _scrollPoolTypes = GUILayout.BeginScrollView(_scrollPoolTypes, false, true);
                    {
                    }
                    GUILayout.EndScrollView();

                    GUI.BeginGroup(GUILayoutUtility.GetLastRect());
                    _drawPoolTypes.DrawWidget(explorer);
                    if (explorer.SelectedPoolType != null)
                    {
                        _drawComponentTypes.PoolType = explorer.SelectedPoolType;
                        _drawComponentTypes.PoolRect = explorer.PoolRect;
                        _drawComponentTypes.DrawWidget(explorer);

                        if (explorer.SelectedComponentType != null)
                        {
                            if (!explorer.SelectedPoolType.IsAssignableFrom(explorer.SelectedComponentType))
                                explorer.SelectedComponentType = null;
                            else
                            {
                                _drawComponentFields.ComponentType = explorer.SelectedComponentType;
                                _drawComponentFields.ComponentRect = explorer.ComponentRect;
                                _drawComponentFields.DrawWidget(explorer);
                            }
                        }
                    }
                    GUI.EndGroup();
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}