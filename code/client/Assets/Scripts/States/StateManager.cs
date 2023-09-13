using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StateManager
    {
        private Dictionary<StateType, StateBase> states = new();

        private StateBase curState;

        public void Init(Main main)
        {
            states.Clear();
            CreateState(main,new SceneState());
            CreateState(main,new ComputerState());
        }

        private void CreateState(Main main, StateBase state)
        {
            state.Main = main;
            states.Add(state.type,state);
        }
        
        public void ChangeToState(StateType stateType)
        {
            curState?.OnExit();
            curState = states[stateType];
            curState?.OnEnter();
        }
        
    }

    public abstract class StateBase
    {
        public Main Main;
        public abstract StateType type { get; }
        public virtual void OnEnter(){}
        public virtual void OnExit(){}
    }

    [Serializable]
    public enum StateType
    {
        Scene,
        Computer
    }

  

    
}