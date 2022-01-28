using UnityEngine.UI;
using IrisFenrir.SaveSystem;
using UnityEngine;

namespace IrisFenrir.Input
{
    // 输入系统管理器
    public class InputManager : Singleton<InputManager>
    {
        public InputData current;
        public bool enable { get; set; }

        private UIKeyManager m_uiMgr;

        // 初始化，在主循环里调用
        public void Init(InputData inputData)
        {
            current = inputData;
            current.Init();
            SaveDefaultSetting();
            m_uiMgr = new UIKeyManager();
            m_uiMgr.CreateUIKeys();

            enable = true;
        }
        // 更新，在主循环的Update里调用
        public void Update()
        {
            if (!enable) return;

            current?.Update();
            m_uiMgr?.Update();
        }

        // 为UIKeyManager设置GR
        public void SetGraphicRaycaster(GraphicRaycaster raycaster)
        {
            if(m_uiMgr != null)
            {
                m_uiMgr.raycaster = raycaster;
            }
        }
        // 更新UI显示，需要时调用一次即可
        public void UpdateUI()
        {
            m_uiMgr?.UpdateUIKeys();
        }

        // 保存、读取默认、自定义设置
        public void SaveDefaultSetting()
        {
            SaveManager.SaveWithJson(current, Application.dataPath + "/Resources/DefaultInput.gu");
        }
        public void LoadDefaultSetting()
        {
            SaveManager.LoadWithJson(ref current, Application.dataPath + "/Resources/DefaultInput.gu");
            UpdateUI();
        }
        public void SaveCustomSetting()
        {
            SaveManager.SaveWithJson(current, Application.dataPath + "/Resources/CustomInput.gu");
        }
        public void LoadCustomSetting()
        {
            if(SaveManager.LoadWithJson(ref current, Application.dataPath + "/Resources/CustomInput.gu"))
            {
                LoadDefaultSetting();
            }
            UpdateUI();
        }

        public static bool GetTap(string name)
        {
            return Instance.current.GetTapKeyState(name);
        }
        public static int GetTapCount(string name)
        {
            return Instance.current.GetTapKeyCount(name);
        }
        public static bool GetPressDown(string name)
        {
            return Instance.current.GetPressKeyDown(name);
        }
        public static bool GetPressUp(string name)
        {
            return Instance.current.GetPressKeyUp(name);
        }
        public static bool GetPressing(string name)
        {
            return Instance.current.GetPressKeyPressing(name);
        }
        public static float GetPressTime(string name)
        {
            return Instance.current.GetPressKeyPressTime(name);
        }
        public static float GetValue(string name)
        {
            return Instance.current.GetValueKeyValue(name);
        }

        public static float GetAxis(string name)
        {
            return Instance.current.GetAxisKeyValue(name);
        }
        public static Vector2 GetAxis2D(string name)
        {
            return Instance.current.GetAxisKeyValue2D(name);
        }

        public static bool GetMultiDown(string name)
        {
            return Instance.current.GetMultiKeyTriggered(name);
        }

        public static bool GetCombo(string name)
        {
            return Instance.current.GetComboKeyTriggered(name);
        }
        public static int GetComboCount(string name)
        {
            return Instance.current.GetComboKeyCombo(name);
        }
    }
}
