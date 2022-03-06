namespace IrisFenrir.EventSystem
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseEvent
    {
        public string Name { get; private set; }
        public bool Enable { get; private set; }

        public BaseEvent() { }
        public BaseEvent(string name)
        {
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public virtual void SetEnable(bool enable, bool includeChildren = true)
        {
            Enable = enable;
        }

        public virtual BaseEvent Find(string name)
        {
            return Name == name ? this : null;
        }

        public abstract void Update();
    }
}
