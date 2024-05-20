using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

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

#if UNITY_EDITOR
        public Vector2 position;
#endif

        public List<Transition> exitTransitions = new List<Transition>();

        [NonSerialized]
        int transitionNextId_ = 0;

        public Dictionary<int, Transition> transitionTable_ = new Dictionary<int, Transition>();

        public void InitTable()
        {
            if (exitTransitions != null)
            {
                foreach (var tarnsition in exitTransitions)
                {
                    if (transitionTable_.ContainsKey(tarnsition.id))
                    {
                        transitionTable_[tarnsition.id] = tarnsition;
                    }
                    else
                    {
                        transitionTable_.Add(tarnsition.id, tarnsition);
                    }
                    transitionNextId_ = tarnsition.id + 1;
                }
            }
        }

        public void SetName(string name)
        {
            this.name = name;
            entity.name_ = name;
        }

        public Transition CheckTransition(float curStateTime)
        {
            foreach(var transition in exitTransitions)
            {
                if (transition.entity.Valid)
                {
                    transition.entity.Execute();
                    if (transition.entity.val_.Bool())
                    {
                        return transition;
                    }
                }
                else
                {
                    if(transition.exitTime <= curStateTime)
                    {
                        return transition;
                    }
                }
            }
            return null;
        }

        public void AddTransition(State destState)
        {
            Transition transition = new Transition()
            {
                id = transitionNextId_,
                previousState = id,
                nextState = destState.id,
                entity = new TransitionEntity(string.Format("{0}->{1}", name, destState.name)),
            };
            transitionNextId_++;
            exitTransitions.Add(transition);
        }

        public void RemoveTransition(int destId)
        {
            var index = exitTransitions.FindIndex(t => t.nextState.Equals(destId));
            exitTransitions.RemoveAt(index);
        }
    }

    [Serializable]
    public class StateEntity : Node_SubGraph, IStateMachinePart
    {
        public StateEntity() : base("State") { }

    }
}
