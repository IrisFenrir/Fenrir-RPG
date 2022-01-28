using UnityEngine;

namespace IrisFenrir.Input
{
    [System.Serializable]
    // 虚拟键基类，所有的功能键都从此派生
    public abstract class VirtualKey
    {
        // 键名，同类型键不要重名
        public string name;
        // 是否启用
        [SerializeField, HideInInspector]
        protected bool enable;

        // 初始化，要在面板赋值，不能使用构造函数，请在使用前调用Init初始化
        public virtual void Init() 
        {
            enable = true;
        }
        // 启用/禁用虚拟键
        public virtual void SetEnable(bool _enable)
        {
            enable = _enable;
        }
        // 更新
        public abstract void Update();
        // 设置键位
        public abstract void SetKeyCode(params KeyCode[] keyCodes);
    }
}

