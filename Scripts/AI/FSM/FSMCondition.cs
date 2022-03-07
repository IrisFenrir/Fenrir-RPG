using System;

namespace IrisFenrir.AI.FSM
{
    public class FSMCondition<T>
    {
        private Func<T, bool> m_conditionHandle;

        public FSMCondition() { }
        public FSMCondition(Func<T, bool> handle)
        {
            BindCondition(handle);
        }

        public void BindCondition(Func<T, bool> handle)
        {
            m_conditionHandle = handle;
        }

        public virtual bool Condition(T owner)
        {
            return m_conditionHandle != null && m_conditionHandle.Invoke(owner);
        }

        public static FSMCondition<T> operator&(FSMCondition<T> con1, FSMCondition<T> con2)
        {
            return new AndCondition<T>(con1, con2);
        }

        public static FSMCondition<T> operator|(FSMCondition<T> con1, FSMCondition<T> con2)
        {
            return new OrCondition<T>(con1, con2);
        }

        public static FSMCondition<T> operator!(FSMCondition<T> con)
        {
            return new NotCondition<T>(con);
        }
    }

    public class FSMCondition<T1,T2> : FSMCondition<T1>
    {
        private Func<T1, T2, bool> m_condion;
        private T2 m_value;

        public FSMCondition() { }
        public FSMCondition(Func<T1, T2, bool> condition, T2 value)
        {
            BindCondition(condition, value);
        }
        
        public void BindCondition(Func<T1, T2, bool> condition, T2 value)
        {
            m_condion = condition;
            m_value = value;
        }

        public override bool Condition(T1 owner)
        {
            return m_condion != null && m_condion(owner, m_value);
        }
    }
}
