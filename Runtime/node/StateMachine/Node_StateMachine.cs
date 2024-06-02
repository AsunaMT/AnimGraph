using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace AnimGraph
{

    [Serializable]
    public class StateMachineCore
    {
        public string name = "StateMachine";
        public int entry = -1;
#if UNITY_EDITOR
        public Vector2 entryPosition;
#endif
        public List<State> states = new List<State>();

        [NonSerialized]
        public int stateNextId_ = 0;

        public Dictionary<int, State> stateTable_ = new Dictionary<int, State>();

        public void InitTable()
        {
            if(states != null)
            {
                foreach(var state in states)
                {
                    if (stateTable_.ContainsKey(state.id))
                    {
                        stateTable_[state.id] = state;
                    }
                    else
                    {
                        stateTable_.Add(state.id, state);
                    }
                    stateNextId_ = state.id + 1;
                    state.InitTable();
                }
            }
        }
    }

    [Serializable]
    public class Node_StateMachine : AnimBehaviourNodeBase, IGraphNode
    {
        public StateMachineCore sm_ = new StateMachineCore();

        public int curState;
        public float curStateTime;
        public List<TransitionBetweenStates> runningTransitions_ = new List<TransitionBetweenStates>();

        public Dictionary<int, float> activeStatesAndWeight_ = new Dictionary<int, float>();
        public AnimationMixerPlayable mixer_ => (AnimationMixerPlayable)mainPlayable_;

        public State entryState => sm_.stateTable_[sm_.entry];

        public string Name => sm_?.name;

        public Node_StateMachine() : base()
        {
            input_ = null;
        }

        public override void InitNode(Animator animator, PlayableGraph graph, Dictionary<string, Variable> variables)
        {
            base.InitNode(animator, graph, variables);
            sm_.InitTable();
            foreach (var state in sm_.states)
            {
                state.entity.InitNode(animator, graph, variables);
                foreach (var transition in state.exitTransitions)
                {
                    transition.entity.InitNode(animator, graph, variables);
                }
            }
            curState = sm_.entry;
        }

        public void InitTable()
        {
            sm_.InitTable();
        }

        public override void CreatePlayable(Animator animator, PlayableGraph graph)
        {
            base.CreatePlayable(animator, graph);
            mainPlayable_ = AnimationMixerPlayable.Create(graph);
            outputPlayable_.AddInput(mainPlayable_, 0, 1f);
            foreach (var state in sm_.states)
            {
                state.entity.CreatePlayable(animator, graph);
            }
        }

        public override void InitConnection(Animator animator, PlayableGraph graph)
        {
            base.InitConnection(animator, graph);
            foreach (var state in sm_.states)
            {
                state.entity.InitConnection(animator, graph);

                foreach (var transition in state.exitTransitions)
                {
                    transition.entity.InitConnection(animator, graph);
                }
                mainPlayable_.AddInput(state.entity.outputPlayable_, 0, 0f);
            }
            mainPlayable_.SetInputWeight(sm_.entry, 1f);
        }

        public override void Execute()
        {
            base.Execute();

            if (sm_ == null || sm_.states.Count == 0) return;

            curStateTime += Time.deltaTime;
            CalcWeight();
            Transition transition = CheckTransition();
            if (transition != null)
            {
                runningTransitions_.Add(new TransitionBetweenStates
                {
                    enterTime = 0f,
                    previousStateWeight = 0f,
                    transition = transition
                });
                if (!activeStatesAndWeight_.ContainsKey(transition.previousState))
                {
                    activeStatesAndWeight_.Add(transition.previousState, 0f);
                }
                if (!activeStatesAndWeight_.ContainsKey(transition.nextState))
                {
                    activeStatesAndWeight_.Add(transition.nextState, 0f);
                }
            }
        }


        void CalcWeight()
        {
            if (runningTransitions_.Count < 1)
            {
                return;
            }
            TransitionBetweenStates main = runningTransitions_[^1];
            main.enterTime += Time.deltaTime;

            if (main.enterTime >= main.transition.transTime || Mathf.Approximately(main.enterTime, main.transition.transTime))
            {
                foreach(var activeState in activeStatesAndWeight_.Keys)
                {
                    mixer_.SetInputWeight(activeState, 0f);
                }
                mixer_.SetInputWeight(main.transition.nextState, 1f);
                curState = main.transition.nextState;
                curStateTime = 0f;
                runningTransitions_.Clear();
                activeStatesAndWeight_.Clear();
                return;
            }

            var keys = new List<int>(activeStatesAndWeight_.Keys);
            foreach (var activeState in keys)
            {
                activeStatesAndWeight_[activeState] = 0f;
            }

            float weight = main.enterTime / main.transition.transTime;
            main.previousStateWeight = 1 - weight;
            activeStatesAndWeight_[main.transition.nextState] = weight;

            for (int i = 0; i < runningTransitions_.Count - 1; )
            {
                var trans = runningTransitions_[i];
                trans.enterTime += Time.deltaTime;
                if (trans.enterTime >= trans.transition.transTime || Mathf.Approximately(trans.enterTime, trans.transition.transTime))
                {
                    runningTransitions_.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            int preCount = runningTransitions_.Count - 1;
            if(preCount > 0 )
            {
                float preOneTransWeight = main.previousStateWeight / preCount;
                for (int i = 0; i < preCount;)
                {
                    var trans = runningTransitions_[i];
                    trans.previousStateWeight -= Time.deltaTime / trans.transition.transTime;
                    activeStatesAndWeight_[trans.transition.previousState] += trans.previousStateWeight * preOneTransWeight;
                    activeStatesAndWeight_[trans.transition.nextState] += (1 - trans.previousStateWeight) * preOneTransWeight;
                }
            }
            else
            {
                activeStatesAndWeight_[main.transition.previousState] = 1 - weight;
            }
            foreach(var active in activeStatesAndWeight_)
            {
                mixer_.SetInputWeight(active.Key, active.Value);
            }
        }

        Transition CheckTransition()
        {
            Transition res;
            if (runningTransitions_.Count > 0)
            {
                Transition trans = runningTransitions_[^1].transition;
                switch (trans.interruptType)
                {
                    case InterruptType.Previous:
                        res = sm_.states[trans.previousState].CheckTransition(curStateTime);
                        break;
                    case InterruptType.Next:
                        res = sm_.states[trans.nextState].CheckTransition(curStateTime);
                        break;
                    case InterruptType.PreviousThenNext:
                        res = sm_.states[trans.previousState].CheckTransition(curStateTime) ?? sm_.states[trans.nextState].CheckTransition(curStateTime);
                        break;
                    case InterruptType.NextThenPrevious:
                        res = sm_.states[trans.nextState].CheckTransition(curStateTime) ?? sm_.states[trans.previousState].CheckTransition(curStateTime);
                        break;
                    default:
                        res = null;
                        break;
                }
            }
            else
            {
                res = sm_.states[curState].CheckTransition(curStateTime);
            }
            return res;
        }

        public void AddTransition(State fromState, State destState)
        {
            Transition transition = new Transition()
            {
                previousState = fromState.id,
                nextState = destState.id,
                entity = new TransitionEntity(string.Format("{0}->{1}", fromState.name, destState.name)),
            };
        }

        public State CreateState()
        {
            State state = new State();
            state.id = sm_.stateNextId_++;
            sm_.states.Add(state);
            sm_.stateTable_.Add(state.id, state);
            return state;
        }

        public State GetState(int id)
        {
            return sm_.stateTable_[id];
        }
    }
}
