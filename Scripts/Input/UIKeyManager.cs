using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IrisFenrir.Input
{
    // 管理所有UIKey，负责与InputData交互
    public class UIKeyManager
    {
        public bool enable { get; set; }
        public GraphicRaycaster raycaster { get; set; }  // UIKey所在Canvas的GraphicRaycaster

        private InputData m_inputData;
        private Transform m_inputWindow;  // 所有UIKey的父物体

        // 通过Text查找对应的UIKey，不必将UIKey挂载到物体上
        private Dictionary<Text,UIKey> m_uiKeys;  
        // 当前接收输入的UIKey，同一时间只能有一个UIKey接收输入，设置完毕后设为null
        private UIKey m_activeKey;
        private List<RaycastResult> m_results;

        public UIKeyManager()
        {
            m_inputData = InputManager.Instance.current;  // 获取InputData
            m_inputWindow = GameObject.Find("Canvas/InputWindow")?.transform;  // 获取UIKey的父物体
            m_uiKeys = new Dictionary<Text, UIKey>();
            enable = true;
        }

        // 遍历键位数据，为每个键生成UIKey，并绑定到对应的Text上
        public void CreateUIKeys()
        {
            if (m_inputData == null || m_inputWindow == null)
                return;

            CreateUITapKeys();
            CreateUIPressKeys();
            CreateUIValueKeys();
            CreateUIAxisKeys();
            CreateUIMultiKeys();
            CreateUIComboKeys();
        }
        // 读取键位数据，更新所有UIKey的显示
        public void UpdateUIKeys()
        {
            foreach (UIKey key in m_uiKeys.Values)
            {
                key.UpdateText();
            }
        }

        // 控制当前激活键设置键位
        public void Update()
        {
            if (!enable) return;
            // 检测鼠标点击的UI
            if(m_activeKey == null && UIRaycaster.RaycastWithClick(raycaster,out m_results))
            {
                Text target = m_results[0].gameObject.GetComponent<Text>();
                if(target != null && m_uiKeys.TryGetValue(target,out UIKey key))
                {
                    if(m_activeKey != null)
                    {
                        m_activeKey.active = false;
                    }
                    m_activeKey = key;
                    key.active = true;
                    return;
                }
            }

            if(m_activeKey != null)
            {
                m_activeKey.AcceptInput();
                if(m_activeKey.active == false)
                {
                    m_activeKey = null;
                }
            }
        }

        private void CreateUITapKeys()
        {
            List<TapKey> tapKeys = m_inputData.tapKeys.keys;
            TapKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < tapKeys.Count; i++)
            {
                key = tapKeys[i];
                CreateOneKey(tapKeys, key, uiKey, key.name, k => key.keyCode = k,
                    k => k.keyCode);
            }
        }
        private void CreateUIPressKeys()
        {
            List<PressKey> pressKeys = m_inputData.pressKeys.keys;
            PressKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < pressKeys.Count; i++)
            {
                key = pressKeys[i];
                CreateOneKey(pressKeys, key, uiKey, key.name, k => key.keyCode = k,
                    k => k.keyCode);
            }
        }
        private void CreateUIValueKeys()
        {
            List<ValueKey> valueKeys = m_inputData.valueKeys.keys;
            ValueKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < valueKeys.Count; i++)
            {
                key = valueKeys[i];
                CreateOneKey(valueKeys, key, uiKey, key.name, k => key.keyCode = k,
                    k => k.keyCode);
            }
        }
        private void CreateUIAxisKeys()
        {
            List<AxisKey> axisKeys = m_inputData.axisKeys.keys;
            AxisKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < axisKeys.Count; i++)
            {
                key = axisKeys[i];
                switch(key.dim)
                {
                    case AxisKey.AxisKeyDimension.Axis1D:
                        CreateOneKey(axisKeys, key, uiKey, key.name + "POS", k => key.posKey = k, k => k.posKey);
                        CreateOneKey(axisKeys, key, uiKey, key.name + "NEG", k => key.negKey = k, k => k.negKey);
                        break;
                    case AxisKey.AxisKeyDimension.Axis2D:
                        CreateOneKey(axisKeys, key, uiKey, key.name + "UP", k => key.upKey = k, k => k.upKey);
                        CreateOneKey(axisKeys, key, uiKey, key.name + "DOWN", k => key.downKey = k, k => k.downKey);
                        CreateOneKey(axisKeys, key, uiKey, key.name + "LEFT", k => key.leftKey = k, k => k.leftKey);
                        CreateOneKey(axisKeys, key, uiKey, key.name + "RIGHT", k => key.rightKey = k, k => k.rightKey);
                        break;
                }
            }
        }
        private void CreateUIMultiKeys()
        {
            List<MultiKey> multiKeys = m_inputData.multiKeys.keys;
            MultiKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < multiKeys.Count; i++)
            {
                key = multiKeys[i];
                if(key != null && key.keys != null)
                {
                    for (int j = 0; j < key.keys.Count; j++)
                    {
                        CreateOneKey(multiKeys, key, uiKey, key.name + j.ToString(),
                            k => key.keys[j] = k,
                            k => k.keys[j]);
                    }
                }
            }
        }
        private void CreateUIComboKeys()
        {
            List<ComboKey> comboKeys = m_inputData.comboKeys.keys;
            ComboKey key = null;
            UIKey uiKey = null;
            for (int i = 0; i < comboKeys.Count; i++)
            {
                key = comboKeys[i];
                if (key != null && key.keys != null)
                {
                    for (int j = 0; j < key.keys.Count; j++)
                    {
                        CreateOneKey(comboKeys, key, uiKey, key.name + j.ToString(),
                            k => key.keys[j] = k,
                            k => k.keys[j]);
                    }
                }
            }
        }

        private void CreateOneKey<T>(List<T> keys,T key,UIKey uiKey,string uiName,
            Action<KeyCode> save,SelectHandler<T,KeyCode> selector)
            where T:VirtualKey
        {
            if (key == null)
                return;
            // 查找显示键位的Text，根据具体层级去写
            Text keyText = m_inputWindow.Find(uiName)?.Find("Code_Text")?.GetComponent<Text>();
            if(keyText != null)
            {
                uiKey = new UIKey(keyText);
                uiKey.SetSaveAction(save);
                uiKey.SetUpdateAction(() => uiKey.SetText(selector(key)));
                m_uiKeys.Add(keyText,uiKey);
            }
        }
    }
}
