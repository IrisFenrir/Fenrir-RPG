using UnityEngine;
using System;

namespace IrisFenrir.Input
{
    // 可在按下时、松开时、持续按下的时候返回true，并可以获得按压时间
    [Serializable]
    public class PressKey : VirtualKey
    {
        // 标记是按下时、持续按下、松开时返回true
        public enum PressKeyType
        {
            Down,
            Pressing,
            Up
        }

        public PressKeyType type;  // 检测类型
        public KeyCode keyCode;

        [HideInInspector] public bool isDown;
        [HideInInspector] public bool isPressing;
        [HideInInspector] public bool isUp;
        [HideInInspector] public float pressTime;

        public override void SetEnable(bool _enable)
        {
            base.SetEnable(_enable);
            isDown = false;
            isPressing = false;
            isUp = false;
            pressTime = 0f;
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
            if (!enable) return;

            switch(type)
            {
                case PressKeyType.Down:
                    isDown = UnityEngine.Input.GetKeyDown(keyCode);
                    break;
                case PressKeyType.Up:
                    isUp = UnityEngine.Input.GetKeyUp(keyCode);
                    break;
                case PressKeyType.Pressing:
                    isPressing = UnityEngine.Input.GetKey(keyCode);
                    if(isPressing)
                    {
                        pressTime += Time.deltaTime;
                    }
                    else
                    {
                        pressTime = 0;
                    }
                    break;
            }
        }
    }
}
