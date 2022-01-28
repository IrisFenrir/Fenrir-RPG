using System;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 按下时从最小值匀速增加到最大值，松开时匀速降到最小值
    [Serializable]
    public class ValueKey : VirtualKey
    {
        // 范围(最小值，最大值)
        public Vector2 range = new Vector2(0f, 1f);
        // 起始点
        public float start = 0f;
        // 增加速度
        public Vector2 speed = 5f * Vector2.one;
        public KeyCode keyCode;

        [HideInInspector] public float value = 0f;

        public override void Init()
        {
            base.Init();
            value = start; // 将value设为起始值
        }

        public override void SetEnable(bool _enable)
        {
            base.SetEnable(_enable);
            value = start;
        }

        public override void SetKeyCode(params KeyCode[] keyCodes)
        {
            if (keyCodes.Length >= 1)
            {
                keyCode = keyCodes[0];
            }
        }

        public override void Update()
        {
            // 确保速度大于0，范围正确，且起始值在范围内
            if (!enable || speed.x <= 0 || speed.y <= 0 || range.x >= range.y ||
                start >= range.y || start <= range.x) return;

            if(UnityEngine.Input.GetKey(keyCode)) // 按下匀速递增
            {
                if(value < range.y)
                {
                    value += speed.x * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, range.y);
                }
            }
            else // 松开匀速递减
            {
                if(value > range.x)
                {
                    value -= speed.y * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, range.y);
                }
            }
        }
    }
}
