using UnityEditor;

namespace Rentitas.Unity
{
    public class TargetSelectionWidget : IKernelExplorerWidget
    {
        private IKernelExplorerWidget _drawExecutor;
        private IKernelExplorerWidget _drawKernel;
        private IKernelExplorerWidget _drawSelectFirst;

        public TargetSelectionWidget()
        {
            _drawExecutor = new DrawExecutorWidget();
            _drawKernel = new DrawKernelWidget();
            _drawSelectFirst = new DrawSelectFirstWidget();
        }

        public void DrawWidget(KernelExplorer explorer)
        {
            explorer.Executor = null;
            explorer.Kernel = null;

            var selection = Selection.activeGameObject;
            if (!selection)
            {
                _drawSelectFirst.DrawWidget(explorer);
                return;
            }

            explorer.Executor = selection.GetComponent<RentitasExecutor>();
            if (explorer.Executor)
            {
                _drawExecutor.DrawWidget(explorer);
                return;
            }

            explorer.Kernel = selection.GetComponent<KernelBehaviour>();
            if (explorer.Kernel)
            {
                _drawKernel.DrawWidget(explorer);
                return;
            }
        }
    }
}