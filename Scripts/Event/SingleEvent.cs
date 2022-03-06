using System;

namespace IrisFenrir.EventSystem
{
    /// <summary>
    /// 单事件，需要绑定条件和行为
    /// </summary>
    public class SingleEvent : BaseEvent
    {
        private Func<bool> m_condition;
        private Action m_action;

        public SingleEvent() { }
        public SingleEvent(string name) : base(name) { }

        public void Register(Func<bool> condition)
        {
            m_condition += condition;
        }
        public void Unregister(Func<bool> condition)
        {
            m_condition -= condition;
        }

        public void Subscribe(Action action)
        {
            m_action += action;
        }
        public void Unsubscribe(Action action)
        {
            m_action -= action;
        }

        public override void Update()
        {
            if (!Enable) return;
            if (m_condition != null && m_condition())
            {
                m_action?.Invoke();
            }
        }
    }

    public class SingleEvent<T> : BaseEvent
    {
        private Func<bool> m_condition;
        private Func<T> m_parameter;
        private Action<T> m_action;

        public SingleEvent() { }
        public SingleEvent(string name) : base(name) { }

        public void Register(Func<bool> condition, Func<T> para)
        {
            m_condition += condition;
            m_parameter += para;
        }
        public void Unregister(Func<bool> condition, Func<T> para)
        {
            m_condition -= condition;
            m_parameter -= para;
        }

        public void Subscribe(Action<T> action)
        {
            m_action += action;
        }
        public void Unsubscribe(Action<T> action)
        {
            m_action -= action;
        }

        public override void Update()
        {
            if (!Enable) return;
            if (m_condition != null && m_condition())
            {
                m_action?.Invoke(m_parameter());
            }
        }
    }

    public class SingleEvent<T1, T2> : BaseEvent
    {
        private Func<bool> m_condition;
        private Func<T1> m_para1;
        private Func<T2> m_para2;
        private Action<T1, T2> m_action;

        public SingleEvent() { }
        public SingleEvent(string name) : base(name) { }

        public void Register(Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            m_condition += condition;
            m_para1 += para1;
            m_para2 += para2;
        }
        public void Unregister(Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            m_condition -= condition;
            m_para1 -= para1;
            m_para2 -= para2;
        }

        public void Subscribe(Action<T1, T2> action)
        {
            m_action += action;
        }
        public void Unsubscribe(Action<T1, T2> action)
        {
            m_action -= action;
        }

        public override void Update()
        {
            if (!Enable) return;
            if (m_condition != null && m_condition())
            {
                m_action?.Invoke(m_para1(), m_para2());
            }
        }
    }
}
