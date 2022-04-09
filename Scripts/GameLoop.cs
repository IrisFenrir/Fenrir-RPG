using IrisFenrir.AnimationSystem;
using IrisFenrir.DialogSystem;
using IrisFenrir.EventSystem;
using IrisFenrir.InputSystem;
using IrisFenrir.InventorySystem;
using IrisFenrir.ItemSystem;
using IrisFenrir.MotionSystem;
using IrisFenrir.TextSystem;
using UnityEngine;
using UnityEngine.UI;

namespace IrisFenrir
{
    public class GameLoop : MonoBehaviour
    {
        [Header("Input System")]
        public InputData inputData;
        public GraphicRaycaster graphicRaycaster;

        [Header("Animation System")]
        public Transform playerModel;
        public AnimSetting animSetting;

        [Header("Motion System")]
        public float walkSpeed = 1f;
        public float runSpeed = 2f;
        public float rotateSpeed = 60f;
        public float jumpForce = 5f;
        public float rollForce = 5f;

        [Header("Inventory System")]
        public Vector3Int inventorySize;
        public Vector3 boxCenter;
        public Vector3 boxSize;

        [Header("Dialog System")]
        public Language language;

        [Header("Debug Info")]
        public string curState;
        public bool isOnGround;
        public Vector2 mouse;

        public PlayerMotion motion;
        private InventoryAsset m_inventory;
        private BoxSelector m_selector;

        public static GameLoop Instance;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            InputManager.Instance.Init(inputData);
            InputManager.Instance.SetGraphicRaycaster(graphicRaycaster);
            InputManager.Instance.LoadCustomSetting();

            EventInit();
            EventManager.Instance.Enable("Root", true);

            ItemFactory.Instance.LoadData("Data/GameItemData");

            motion = new PlayerMotion(animSetting);
            m_selector = new BoxSelector(playerModel, boxCenter, boxSize);
            m_inventory = new InventoryAsset(inventorySize, "InventoryWindow", 
                m_selector);

            InventoryManager.Instance.Add(m_inventory);
        }

        private void Update()
        {
            InputManager.Instance.Update();
            EventManager.Instance.Update();
            DialogManager.Instance.Update();

            motion.Update();
            
            m_inventory.Update();
            

            mouse = Input.mousePosition;
        }

        private void OnDisable()
        {
            motion.Stop();
        }

        private void EventInit()
        {
            EventManager.Instance.CreateEventGroup("Inventory");
            EventManager.Instance.CreateTapKeyEvent("Pick", "Inventory");
            EventManager.Instance.CreateTapKeyEvent("OpenInv", "Inventory");

            EventManager.Instance.Enable("Inventory", true, true);
            EventManager.Instance.Enable("InvInner", false);

            EventManager.Instance.CreateEventGroup("State");
            EventManager.Instance.CreateEvent<float>("OnHealthChanged", "State");
            EventManager.Instance.CreateEvent<float>("OnManaChanged", "State");
            EventManager.Instance.Enable("State", true, true);
        }
    }
}
