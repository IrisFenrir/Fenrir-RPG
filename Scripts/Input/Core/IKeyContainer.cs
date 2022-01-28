using UnityEngine;

namespace IrisFenrir.Input
{
    // 虚拟键容器接口
    public interface IKeyContainer
    {
        VirtualKey GetKeyObject(string name);  // 获取虚拟键对象
        void SetKeyCode(string name, params KeyCode[] keyCodes);  // 设置键值
        void EnableKey(string name, bool enable);  // 启用虚拟键
        void Init();  // 初始化
        void Update();  // 更新
    }
}
