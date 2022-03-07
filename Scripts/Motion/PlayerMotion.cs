using IrisFenrir.AnimationSystem;
using UnityEngine;

namespace IrisFenrir.MotionSystem
{
    public class PlayerMotion
    {
        public PlayerAI AI { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerParam Param { get; private set; }
        public PlayerMotor Motor { get; private set; }
        public PlayerAnim Anim { get; private set; }
        public Transform Model { get; private set; }

        public PlayerMotion(AnimSetting animSetting)
        {
            Param = new PlayerParam();
            Model = GetPlayerModel();
            Anim = new PlayerAnim(this, animSetting);
            Input = new PlayerInput(this);
            Motor = new PlayerMotor(this);
            AI = new PlayerAI(this);
        }

        public void Update()
        {
            DetectGround();

            Input.Update();
            Anim.Update();
            AI.Update();
            GameLoop.Instance.curState = AI.currentStateName;
        }

        public void Enable(bool enable)
        {
            Input.Enable(enable);
        }

        public void Stop()
        {
            Anim.Stop();
        }

        private Transform GetPlayerModel()
        {
            return GameLoop.Instance.playerModel;
        }

        private void DetectGround()
        {
            Param.isOnGround = OnGroundSensor.IsOnGround(Model.GetComponent<CapsuleCollider>(), 0.005f);
            GameLoop.Instance.isOnGround = Param.isOnGround;
        }
    }
}
