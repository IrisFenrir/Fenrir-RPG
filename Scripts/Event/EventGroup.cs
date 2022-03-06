using System.Collections.Generic;

namespace IrisFenrir.EventSystem
{
    /// <summary>
    /// 事件组，不绑定任何方法，负责管理子事件
    /// </summary>
    public class EventGroup : BaseEvent
    {
        private List<BaseEvent> m_children;

        public EventGroup() { }
        public EventGroup(string name) : base(name) { }

        public void AddEvent(BaseEvent e)
        {
            if (e == null) return;
            m_children ??= new List<BaseEvent>();
            m_children.Add(e);
        }

        public bool RemoveEvent(BaseEvent e)
        {
            return m_children != null && m_children.Remove(e);
        }

        public override void SetEnable(bool enable, bool includeChildren = true)
        {
            base.SetEnable(enable);
            if (!includeChildren) return;
            foreach (var e in m_children)
            {
                e.SetEnable(enable);
            }
        }

        public override BaseEvent Find(string name)
        {
            if (Name == name) return this;
            BaseEvent target = null;
            foreach (var e in m_children)
            {
                target = e.Find(name);
                if (target != null) break;
            }
            return target;
        }

        public override void Update()
        {
            if (!Enable) return;
            foreach (BaseEvent e in m_children)
            {
                e.Update();
            }
        }
    }
}
