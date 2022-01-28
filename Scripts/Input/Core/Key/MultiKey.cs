using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 组合键，检测同时按下多个键，如Ctrl + C
    [Serializable]
    public class MultiKey : VirtualKey
    {
        public int count = 2;  // 组合键的数量
        public List<KeyCode> keys; // 待检测的键
        public float interval = 0.5f; // 不同键之间的最大间隔

        [HideInInspector] public bool isTriggered;

        [SerializeField, HideInInspector]
        private float m_currentInterval;
        [SerializeField, HideInInspector]
        private bool[] m_keyState;  // 标记每个键是否按下

        public override void Init()
        {
            base.Init();
            m_keyState = new bool[count]; // 默认全为false
            for (int i = 0; i < count; i++)
            {
                m_keyState[i] = false;
            }
        }

        public override void SetEnable(bool _enable)
        {
            base.SetEnable(_enable);
            isTriggered = false;
            Reset();
        }

        public override void SetKeyCode(params KeyCode[] keyCodes)
        {
            keys.Clear();  // 设置时原本的键全部清空，设为接收的新键
            for (int i = 0; i < keyCodes.Length; i++)
            {
                keys.Add(keyCodes[i]);
            }
        }

        public override void Update()
        {
            // 检查键的数量
            if (!enable || keys == null || keys.Count < count || m_keyState == null)
                return;

            isTriggered = false;
            m_currentInterval += Time.deltaTime;
            // 指定时间按下所有键，则触发
            // 一帧按下所有键触发较为困难，因此选择指定时间内这种较为宽松的方式
            if(m_currentInterval <= interval)
            {
                for (int i = 0; i < count; i++)
                {
                    if(UnityEngine.Input.GetKeyDown(keys[i]))
                    {
                        m_keyState[i] = true;
                    }
                }

                if(AllKeyTriggered())
                {
                    isTriggered = true;
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }

        // 检测所有键是否触发
        private bool AllKeyTriggered()
        {
            for (int i = 0; i < count; i++)
            {
                if(m_keyState[i] == false)
                {
                    return false;
                }
            }
            return true;
        }

        // 重置
        private void Reset()
        {
            m_currentInterval = 0f;
            for (int i = 0; i < count; i++)
            {
                m_keyState[i] = false;
            }
        }
    }
}
