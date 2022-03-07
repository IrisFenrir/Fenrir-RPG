using System;
using System.Collections.Generic;

namespace IrisFenrir.AI.FSM
{
    public class FSMState<T>
    {
        private Dictionary<FSMCondition<T>, string> m_conditionMaps;

        private Action<T> m_enterHandle;
        private Action<T> m_updateHandle;
        private Action<T> m_exitHandle;

        public void BindEnterAction(Action<T> action)
        {
            m_enterHandle = action;
        }
        public void BindUpdateAction(Action<T> action)
        {
            m_updateHandle = action;
        }
        public void BindExitAction(Action<T> action)
        {
            m_exitHandle = action;
        }

        public void AddConditon(FSMCondition<T> condition, string stateName)
        {
            if(condition == null || string.IsNullOrEmpty(stateName))
            {
                return;
            }
            if (m_conditionMaps == null)
                m_conditionMaps = new Dictionary<FSMCondition<T>, string>();
            m_conditionMaps.Add(condition, stateName);
        }
        public void RemoveCondition(FSMCondition<T> condition)
        {
            if (m_conditionMaps == null || condition == null)
                return;
            m_conditionMaps.Remove(condition);
        }
        public bool CheckCondition(T owner, out string stateName)
        {
            if (m_conditionMaps == null)
            {
                stateName = string.Empty;
                return false;
            }
            foreach (var condition in m_conditionMaps.Keys)
            {
                if(condition.Condition(owner))
                {
                    stateName = m_conditionMaps[condition];
                    return true;
                }
            }
            stateName = string.Empty;
            return false;
        }

        public virtual void OnEnter(T owner) 
        {
            if(m_enterHandle != null)
                m_enterHandle(owner);
        }
        public virtual void OnUpdate(T owner)
        {
            if (m_updateHandle != null)
                m_updateHandle(owner);
        }
        public virtual void OnExit(T owner)
        {
            if (m_exitHandle != null)
                m_exitHandle(owner);
        }
    }
}
