using System;
using UnityEngine;
using UnityEngine.UI;

namespace IrisFenrir.Input
{
    // 显示键位设置的UI
    public class UIKey
    {
        // 为true则开始接收输入，并将输入设为新键值
        public bool active { get; set; }

        private Text m_text;  // 显示键位的Text
        private KeyCode m_keyCode;
        private Action<KeyCode> m_saveData;  // 将设置的新键存到VirtualKey对象中
        private Action m_updateText;  // 读取VirtualKey对象中的设置，并更新UI显示

        public UIKey(Text text)
        {
            m_text = text;
        }
        public void SetSaveAction(Action<KeyCode> save)
        {
            if(save != null)
            {
                m_saveData = save;
            }
        }
        public void SetUpdateAction(Action update)
        {
            if (update != null)
            {
                m_updateText = update;
            }
        }

        // 接收输入
        public void AcceptInput()
        {
            if (!active) return;

            if(InputHelper.GetInputKey(out m_keyCode))
            {
                // 检测到输入则设置UI，更新数据，并将active设为false，代表设置完毕
                m_text.text = m_keyCode.ToString();
                m_saveData(m_keyCode);
                active = false;
            }
            else
            {
                // 未输入时显示None
                m_text.text = "None";
            }
        }

        // 更新Text
        public void UpdateText()
        {
            if(m_updateText != null)
            {
                m_updateText();
            }
        }

        public void SetText(KeyCode keyCode)
        {
            m_text.text = keyCode.ToString();
        }
    }
}
