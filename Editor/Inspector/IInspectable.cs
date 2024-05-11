
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimGraph.Editor
{
    public interface IInspectable
    {
        public string TypeName => GetType().Name;

        public IInspector GetInspector();
    }
}
