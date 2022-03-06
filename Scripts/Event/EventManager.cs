using System;

namespace IrisFenrir.EventSystem
{
    public class EventManager : Singleton<EventManager>
    {
        private EventGroup m_root = new EventGroup("Root");

        public SingleEvent CreateEvent(string name, string parent = "Root")
        {
            return Create<SingleEvent>(name, parent);
        }
        public SingleEvent<T> CreateEvent<T>(string name, string parent = "Root")
        {
            return Create<SingleEvent<T>>(name, parent);
        }
        public SingleEvent<T1, T2> CreateEvent<T1, T2>(string name, string parent = "Root")
        {
            return Create<SingleEvent<T1, T2>>(name, parent);
        }

        public EventGroup CreateEventGroup(string name, string parent = "Root")
        {
            return Create<EventGroup>(name, parent);
        }

        public bool Register(string name, Func<bool> condition)
        {
            if (!(m_root.Find(name) is SingleEvent e)) return false;
            e.Register(condition);
            return true;
        }
        public bool Register<T>(string name, Func<bool> condition, Func<T> para)
        {
            if (!(m_root.Find(name) is SingleEvent<T> e)) return false;
            e.Register(condition, para);
            return true;
        }
        public bool Register<T1, T2>(string name, Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            if (!(m_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Register(condition, para1, para2);
            return true;
        }

        public bool Unregister(string name, Func<bool> condition)
        {
            if (!(m_root.Find(name) is SingleEvent e)) return false;
            e.Unregister(condition);
            return true;
        }
        public bool Unregister<T>(string name, Func<bool> condition, Func<T> para)
        {
            if (!(m_root.Find(name) is SingleEvent<T> e)) return false;
            e.Unregister(condition, para);
            return true;
        }
        public bool Unregister<T1, T2>(string name, Func<bool> condition, Func<T1> para1, Func<T2> para2)
        {
            if (!(m_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Unregister(condition, para1, para2);
            return true;
        }

        public bool Subscribe(string name, Action action)
        {
            if (!(m_root.Find(name) is SingleEvent e)) return false;
            e.Subscribe(action);
            return true;
        }
        public bool Subscribe<T>(string name, Action<T> action)
        {
            if (!(m_root.Find(name) is SingleEvent<T> e)) return false;
            e.Subscribe(action);
            return true;
        }
        public bool Subscribe<T1, T2>(string name, Action<T1, T2> action)
        {
            if (!(m_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Subscribe(action);
            return true;
        }

        public bool Unsubscribe(string name, Action action)
        {
            if (!(m_root.Find(name) is SingleEvent e)) return false;
            e.Unsubscribe(action);
            return true;
        }
        public bool Unsubscribe<T>(string name, Action<T> action)
        {
            if (!(m_root.Find(name) is SingleEvent<T> e)) return false;
            e.Unsubscribe(action);
            return true;
        }
        public bool Unsubscribe<T1, T2>(string name, Action<T1, T2> action)
        {
            if (!(m_root.Find(name) is SingleEvent<T1, T2> e)) return false;
            e.Unsubscribe(action);
            return true;
        }

        public void Enable(string name, bool enable, bool includeChildren = true)
        {
            var target = m_root.Find(name);
            if (target == null) return;
            target.SetEnable(enable, includeChildren);
        }

        public void Update()
        {
            m_root?.Update();
        }

        private T Create<T>(string name, string parent)
            where T:BaseEvent, new()
        {
            if (!(m_root.Find(parent) is EventGroup group)) return null;
            var e = new T();
            e.SetName(name);
            group.AddEvent(e);
            return e;
        }
    }
}
