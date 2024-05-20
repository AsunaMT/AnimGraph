using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public float exitTime;
        public float transTime;
        public TransitionEntity entity = new TransitionEntity();
        public AnimationCurve blendCurve;

        public bool ContainsState(int id)
        {
            return previousState == id || nextState == id;
        }
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
        public TransitionEntity() : base("Transition", PinType.EBool) { }


        public TransitionEntity(string name) : base(name, PinType.EBool) { }
    }
}
