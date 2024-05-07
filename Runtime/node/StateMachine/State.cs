using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph
{

    public class ActiveState
    {
        public State state;
        public float weight;
    }

    [Serializable]
    public class State
    {
        public int id;
        public string name;
        public StateEntity entity = new StateEntity();
        public List<Transition> exitTransitions = new List<Transition>();

        public Transition CheckTransition()
        {
            foreach(var transition in exitTransitions)
            {
                transition.entity.Execute();
                if (transition.entity.val_.Bool())
                {
                    return transition;
                }
            }
            return null;
        }
    }

    public class StateEntity : Node_SubGraph, IStateMachinePart
    {

    }
}
