using System;
using System.Collections.Generic;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 检测任意数量任意键的连击
    [Serializable]
    public class ComboKey : VirtualKey
    {
        public List<KeyCode> keys;  // 待检测的键
        public float interval = 0.5f;  // 两个键之间的间隔

        [HideInInspector] public bool isTriggered;
        [HideInInspector] public int combo;  // 连击数

        [SerializeField, HideInInspector]
        private float m_currentInterval;

        public override void SetEnable(bool _enable)
        {
            base.SetEnable(_enable);
            isTriggered = false;
            combo = 0;
            m_currentInterval = 0f;
        }

        public override void SetKeyCode(params KeyCode[] keyCodes)
        {
            keys.Clear();
            for (int i = 0; i < keyCodes.Length; i++)
            {
                keys.Add(keyCodes[i]);
            }
        }

        public override void Update()
        {
            if (!enable || keys == null || keys.Count <= 0 || interval <= 0f)
                return;

            isTriggered = false;
            m_currentInterval += Time.deltaTime;
            if(m_currentInterval <= interval)
            {
                if(InputHelper.GetInputKey(out KeyCode curKey))
                {
                    m_currentInterval = 0f;
                    if (curKey == keys[combo])
                    {
                        combo++;
                        if (combo == keys.Count)
                        {
                            isTriggered = true;
                            combo = 0;
                        }
                    }
                    else
                    {
                        combo = 0;
                    }
                }
            }
            else
            {
                combo = 0;
                m_currentInterval = 0f;
            }
        }
    }
}
