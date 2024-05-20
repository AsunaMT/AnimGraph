using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph.Editor
{
    public partial class AnimGraphEditorWindow
    {
        private AnimGraphEditorVarListManager varListManager;
        private void CreateVarListPanel()
        {
            varListManager = new AnimGraphEditorVarListManager(_layoutContainer.LeftPane);
        }

    }
}
