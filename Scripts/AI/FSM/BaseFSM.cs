using System.Collections.Generic;

namespace IrisFenrir.AI.FSM
{
    public class BaseFSM<T>
    {
        public string currentStateName;

        private T m_owner;
        private Dictionary<string, FSMState<T>> m_states;

        private FSMState<T> m_defaultState;
        private FSMState<T> m_currentState;

        private bool m_isInit = false;

        public BaseFSM(T owner)
        {
            m_owner = owner;
            m_states = new Dictionary<string, FSMState<T>>();
        }

        public void Update()
        {
            Init();

            if(m_currentState != null)
            {
                m_currentState.OnUpdate(m_owner);
                if(m_currentState.CheckCondition(m_owner,out string stateName))
                {
                    ChangeState(stateName);
                    currentStateName = stateName;
                }
            }
        }

        public void SetDefault(string stateName)
        {
            if(m_states.ContainsKey(stateName))
            {
                m_defaultState = m_states[stateName];
                m_currentState = m_defaultState;
                currentStateName = stateName;
            }
        }
        public void AddState(string stateName,FSMState<T> state)
        {
            if(string.IsNullOrEmpty(stateName) || state == null)
            {
                return;
            }
            m_states.Add(stateName, state);
        }

        private void Init()
        {
            if(!m_isInit)
            {
                m_currentState.OnEnter(m_owner);
                m_isInit = true;
            }
        }
        private void ChangeState(string stateName)
        {
            if(m_states.TryGetValue(stateName,out FSMState<T> state))
            {
                m_currentState.OnExit(m_owner);
                state.OnEnter(m_owner);
                m_currentState = state;
            }
        }
    }
}
