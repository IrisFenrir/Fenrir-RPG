using System;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 输入帮助类
    public class InputHelper
    {
        private static Event m_Event;

        // 在OnGUI中获取按下的键
        public static bool GetInputKeyOnGUI(out KeyCode key,Event e = null)
        {
            key = KeyCode.None;

            if(e != null)
            {
                m_Event = e;
            }
            else if(m_Event == null)
            {
                m_Event = Event.current;
            }

            if(m_Event == null)
            {
                return false;
            }

            if (UnityEngine.Input.anyKey)
            {
                if (m_Event.type == EventType.KeyDown && m_Event.keyCode != KeyCode.None)
                {
                    key = m_Event.keyCode;
                    return true;
                }
                else if(m_Event.type == EventType.MouseDown)
                {
                    key = TypeUtility.StringToEnum<KeyCode>("Mouse" + m_Event.button.ToString());
                    return true;
                }
            }
            return false;
        }

        // 在任意地方获取按下的键
        public static bool GetInputKey(out KeyCode key)
        {
            key = KeyCode.None;
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if(UnityEngine.Input.GetKeyDown(keyCode))
                {
                    key = keyCode;
                    return true;
                }
            }
            return false;
        }

    }
}
