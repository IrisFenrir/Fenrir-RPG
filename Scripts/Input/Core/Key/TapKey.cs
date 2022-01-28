using System;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 点击键，可以检测某个键任意次数点击
    [Serializable]
    public class TapKey : VirtualKey
    {
        // 点击次数
        public int clickCount = 1;
        // 两次点击之间的最大间隔时间
        public float clickInterval = 0.5f;
        // 键值
        public KeyCode keyCode; 
        // 是否触发
        [HideInInspector] public bool isTriggered;
        // 当前点击次数
        [HideInInspector] public int currentCount;

        // 当前点击间隔
        [SerializeField, HideInInspector]
        private float m_currentClickInterval;

        // 每次启用禁用时，将一些变量初始化
        public override void SetEnable(bool _enable)
        {
            base.SetEnable(_enable);
            isTriggered = false;
            currentCount = 0;
            m_currentClickInterval = 0f;
        }
        // 设置键位
        public override void SetKeyCode(params KeyCode[] keyCodes)
        {
            // 确保至少传入了一个键
            if(keyCodes.Length >= 1)
            {
                keyCode = keyCodes[0];
            }
        }
        // 更新
        public override void Update()
        {
            // 启用，并且clickCount >= 1 才执行更新
            if (!enable || clickCount < 1) return;

            // 先将IsTriggered默认设为false
            isTriggered = false;
            // 如果只检测单击，直接获取
            if(clickCount == 1)
            {
                isTriggered = UnityEngine.Input.GetKeyDown(keyCode);
            }
            else if(clickInterval > 0f) // 多次点击时，先检查间隔是否大于0
            {
                // 当前间隔递增
                m_currentClickInterval += Time.deltaTime;
                if (m_currentClickInterval <= clickInterval)
                {
                    // 未超时，检测按下keyCode
                    if(UnityEngine.Input.GetKeyDown(keyCode))
                    {
                        // 按下后计数，重置间隔，达标后重置计数
                        currentCount++;
                        m_currentClickInterval = 0f; 
                        if (currentCount >= clickCount) 
                        {
                            isTriggered = true;
                            currentCount = 0;
                        }
                    }
                }
                else // 超时直接重置
                {
                    currentCount = 0;
                    m_currentClickInterval = 0f;
                }
            }

        }
    }
}
