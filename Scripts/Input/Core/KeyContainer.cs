using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 虚拟键容器，通过接口实现多态，主要定义一些通用方法，如查找键、设置键值、启用键
    [Serializable]
    public class KeyContainer<T> : IKeyContainer
        where T:VirtualKey, new()
    {
        public List<T> keys;

        public VirtualKey GetKeyObject(string name)
        {
            if (keys == null)
                return null;
            return keys.Find(key => key.name == name);
        }

        public void SetKeyCode(string name,params KeyCode[] keyCodes)
        {
            VirtualKey key = GetKeyObject(name);
            key?.SetKeyCode(keyCodes);
        }

        public void EnableKey(string name,bool enable)
        {
            VirtualKey key = GetKeyObject(name);
            key?.SetEnable(enable);
        }

        public void Init()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].Init();
            }
            
        }

        public void Update()
        {
            if (keys == null)
                return;
            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].Update();
            }
        }
    }
}
