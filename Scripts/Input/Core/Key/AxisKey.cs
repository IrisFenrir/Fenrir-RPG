using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 设置一组控制轴向的键，可以选择一维或二维
    [Serializable]
    public class AxisKey : VirtualKey
    {
        // 维度，一维或二维
        public enum AxisKeyDimension
        {
            Axis1D,
            Axis2D
        }
        // 类型，立即变化或渐进变化
        public enum AxisKeyType
        {
            Gradual,
            Sudden
        }

        #region 变量
        // 以下使用了Odin插件扩展的特性EnumPaging和ShowIf，删除不会影响功能
        [EnumPaging]
        public AxisKeyDimension dim = AxisKeyDimension.Axis1D; // 维度
        [EnumPaging]
        public AxisKeyType type = AxisKeyType.Gradual; // 变化类型

        // 一维轴的设置，根据dim类型选择使用哪组设置
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        [Header("Axis 1D")]
        public KeyCode posKey;
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        public KeyCode negKey;
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        public Vector2 range = new Vector2(-1, 1);
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        public float start = 0f;
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        public Vector2 posSpeed = Vector2.one * 5f;
        [ShowIf("dim", AxisKeyDimension.Axis1D)]
        public Vector2 negSpeed = Vector2.one * 5f;

        // 二维轴的设置
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        [Header("Axis 2D")]
        public KeyCode upKey;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public KeyCode downKey;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public KeyCode leftKey;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public KeyCode rightKey;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 horRange = new Vector2(-1, 1);
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 verRange = new Vector2(-1, 1);
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public float horStart = 0f;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public float verStart = 0f;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 horPosSpeed = Vector2.one * 5f;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 horNegSpeed = Vector2.one * 5f;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 verPosSpeed = Vector2.one * 5f;
        [ShowIf("dim", AxisKeyDimension.Axis2D)]
        public Vector2 verNegSpeed = Vector2.one * 5f;

        // 调用与dim相对应的变量才能获取正确值
        // 不要dim设为1D，去调value2D的值
        [HideInInspector] public float value1D;
        [HideInInspector] public Vector2 value2D;
        #endregion

        public override void Init()
        {
            base.Init();
            value1D = start;
            value2D.Set(horStart, verStart);
        }

        public override void SetKeyCode(params KeyCode[] keyCodes)
        {
            // 传入4个键以上，优先为Axis2D设置
            // 需要为Axis1D设置时，请传入两个键，不要超过3个键
            if (keyCodes.Length >= 4)
            {
                upKey = keyCodes[0];
                downKey = keyCodes[1];
                leftKey = keyCodes[2];
                rightKey = keyCodes[3];
            }
            else if (keyCodes.Length >= 2)
            {
                posKey = keyCodes[0];
                negKey = keyCodes[1];
            }
        }

        public override void Update()
        {
            // 检查速度和范围
            if (!enable || !Check(posSpeed, range, start) || !Check(negSpeed, range, start) || 
                !Check(horPosSpeed, horRange, horStart) || !Check(horNegSpeed, horRange, horStart) ||
                !Check(verPosSpeed, horRange, horStart) || !Check(verNegSpeed, horRange, horStart))
                return;

            // 针对不同维度和类型分别处理
            switch(dim)
            {
                case AxisKeyDimension.Axis1D:
                    switch(type)
                    {
                        case AxisKeyType.Sudden:
                            SetSuddenValue(ref value1D, start, range, posKey, negKey);
                            break;
                        case AxisKeyType.Gradual:
                            SetGradualValue(ref value1D, start, posSpeed, negSpeed, range, posKey, negKey);
                            break;
                    }
                    break;
                case AxisKeyDimension.Axis2D:
                    switch(type)
                    {
                        case AxisKeyType.Sudden:
                            SetSuddenValue(ref value2D.x, horStart, horRange, rightKey, leftKey);
                            SetSuddenValue(ref value2D.y, verStart, verRange, upKey, downKey);
                            break;
                        case AxisKeyType.Gradual:
                            SetGradualValue(ref value2D.x, horStart, horPosSpeed, horNegSpeed, horRange, rightKey, leftKey);
                            SetGradualValue(ref value2D.y, verStart, verPosSpeed, verNegSpeed, verRange, upKey, downKey);
                            break;
                    }
                    break;
            }
        }
        // 检查速度和范围
        private bool Check(Vector2 speed,Vector2 range,float start)
        {
            return speed.x > 0 && speed.y > 0 && range.x < range.y && start > range.x && start < range.y;
        }

        // 按下pos，value立刻变为最大值；按下neg，value立刻变为最小值；松开时，value立刻变为初始值
        private void SetSuddenValue(ref float value,float start,Vector2 range,KeyCode pos,KeyCode neg)
        {
            if (UnityEngine.Input.GetKey(pos))
            {
                value = range.y;
            }
            else if (UnityEngine.Input.GetKey(neg))
            {
                value = range.x;
            }
            else
            {
                value = start;
            }
        }

        // 同SetSuddenValue，过程改为渐进
        private void SetGradualValue(ref float value, float start, Vector2 speed,
            Vector2 range, KeyCode pos, KeyCode neg)
        {
            if (UnityEngine.Input.GetKey(pos))
            {
                value += speed.x * Time.deltaTime;
                value = Mathf.Clamp(value, range.x, range.y);
            }
            else if (UnityEngine.Input.GetKey(neg))
            {
                value -= speed.x * Time.deltaTime;
                value = Mathf.Clamp(value, range.x, range.y);
            }
            else
            {
                if (value > 0)
                {
                    value -= speed.y * Time.deltaTime;
                    value = Mathf.Clamp(value, start, range.y);
                }
                if (value < 0)
                {
                    value += speed.y * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, start);
                }
            }

        }

        private void SetGradualValue(ref float value, float start, Vector2 posSpeed, Vector2 negSpeed,
            Vector2 range, KeyCode pos, KeyCode neg)
        {
            if (UnityEngine.Input.GetKey(pos))
            {
                if(value >= start)
                {
                    value += posSpeed.x * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, range.y);
                }
                else
                {
                    value += negSpeed.y * Time.deltaTime;
                }
            }
            else if (UnityEngine.Input.GetKey(neg))
            {
                if(value > start)
                {
                    value -= posSpeed.y * Time.deltaTime;
                    
                }
                else
                {
                    value -= negSpeed.x * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, range.y);
                }
            }
            else
            {
                if (value > start)
                {
                    value -= posSpeed.y * Time.deltaTime;
                    value = Mathf.Clamp(value, start, range.y);
                }
                if (value < 0)
                {
                    value += negSpeed.y * Time.deltaTime;
                    value = Mathf.Clamp(value, range.x, start);
                }
            }
        }
    }
}
