using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 存放键位数据的对象，同时定义一些每种键的特殊方法
    [CreateAssetMenu(fileName = "New Input Data",menuName = "GUGame/InputSystem/InputData")]
    public class InputData : ScriptableObject
    {
        // 各种虚拟键容器
        public KeyContainer<TapKey> tapKeys;
        public KeyContainer<PressKey> pressKeys;
        public KeyContainer<ValueKey> valueKeys;
        public KeyContainer<AxisKey> axisKeys;
        public KeyContainer<MultiKey> multiKeys;
        public KeyContainer<ComboKey> comboKeys;

        // 通过类型名查找对应的容器
        private Dictionary<string, IKeyContainer> m_keyContainerMap;
        private Dictionary<string, IKeyContainer> KeyMap
        {
            get
            {
                if(m_keyContainerMap == null)
                {
                    InitKeyMap();
                }
                return m_keyContainerMap;
            }
        }
        private void InitKeyMap()
        {
            m_keyContainerMap = new Dictionary<string, IKeyContainer>();
            m_keyContainerMap.Add("TapKey", tapKeys);
            m_keyContainerMap.Add("PressKey", pressKeys);
            m_keyContainerMap.Add("ValueKey", valueKeys);
            m_keyContainerMap.Add("AxisKey", axisKeys);
            m_keyContainerMap.Add("MultiKey", multiKeys);
            m_keyContainerMap.Add("ComboKey", comboKeys);
        }

        public void Init()
        {
            foreach (var container in KeyMap.Values)
            {
                container.Init();
            }
        }
        public void Update()
        {
            foreach (var container in KeyMap.Values)
            {
                container?.Update();
            }
        }

        // 设置键位
        public void SetKeyCode<T>(string name,params KeyCode[] keyCodes)
            where T:VirtualKey
        {
            KeyMap[typeof(T).Name]?.SetKeyCode(name, keyCodes);
        }

        // 启用键
        public void EnableKey<T>(string name,bool enable)
        {
            KeyMap[typeof(T).Name]?.EnableKey(name, enable);
        }

        // 在容器中查找键，并返回指定的属性
        private TValue GetKeyProperty<TKey,TValue>(string name,IKeyContainer container, SelectHandler<TKey,TValue> selector)
            where TKey:VirtualKey
        {
            TKey key = container.GetKeyObject(name) as TKey;
            if (key == null)
                return default;
            return selector(key);
        }

        // 读取各类键的属性
        #region TapKey
        public bool GetTapKeyState(string name)
        {
            return GetKeyProperty<TapKey, bool>(name, tapKeys, k => k.isTriggered);
        }
        public int GetTapKeyCount(string name)
        {
            return GetKeyProperty<TapKey, int>(name, tapKeys, k => k.currentCount);
        }
        #endregion

        #region PressKey
        public bool GetPressKeyDown(string name)
        {
            return GetKeyProperty<PressKey, bool>(name, pressKeys, k => k.isDown);
        }
        public bool GetPressKeyUp(string name)
        {
            return GetKeyProperty<PressKey, bool>(name, pressKeys, k => k.isUp);
        }
        public bool GetPressKeyPressing(string name)
        {
            return GetKeyProperty<PressKey, bool>(name, pressKeys, k => k.isPressing);
        }
        public float GetPressKeyPressTime(string name)
        {
            return GetKeyProperty<PressKey, float>(name, pressKeys, k => k.pressTime);
        }
        #endregion

        #region ValueKey
        public float GetValueKeyValue(string name)
        {
            return GetKeyProperty<ValueKey, float>(name, valueKeys, k => k.value);
        }
        #endregion

        #region AxisKey
        public float GetAxisKeyValue(string name)
        {
            return GetKeyProperty<AxisKey, float>(name, axisKeys, k => k.value1D);
        }
        public Vector2 GetAxisKeyValue2D(string name)
        {
            return GetKeyProperty<AxisKey, Vector2>(name, axisKeys, k => k.value2D);
        }
        #endregion

        #region MultiKey
        public bool GetMultiKeyTriggered(string name)
        {
            return GetKeyProperty<MultiKey, bool>(name, multiKeys, k => k.isTriggered);
        }
        #endregion

        #region ComboKey
        public bool GetComboKeyTriggered(string name)
        {
            return GetKeyProperty<ComboKey, bool>(name, comboKeys, k => k.isTriggered);
        }
        public int GetComboKeyCombo(string name)
        {
            return GetKeyProperty<ComboKey, int>(name, comboKeys, k => k.combo);
        }
        #endregion
    }
}
