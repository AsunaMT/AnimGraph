using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimGraph
{
    public enum InterruptType
    {
        None,
        Previous,
        Next,
        PreviousThenNext,
        NextThenPrevious
    }


    [Serializable]
    public class Transition
    {
        public int id;
        public int previousState;
        public int nextState;
        public InterruptType interruptType;
        public float transTime;
        public TransitionEntity entity = new TransitionEntity();
    }

    public class TransitionBetweenStates
    {
        public float enterTime;
        public float previousStateWeight;
        public Transition transition;
    }

    [Serializable]
    public class TransitionEntity : Node_Function, IStateMachinePart
    {

    }
}
