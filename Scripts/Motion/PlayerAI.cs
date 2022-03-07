using IrisFenrir.AI.FSM;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace IrisFenrir.MotionSystem
{
    public class PlayerAI
    {
        public string currentStateName => m_fsm.currentStateName;

        private BaseFSM<PlayerMotion> m_fsm;

        public PlayerAI(PlayerMotion motion)
        {
            m_fsm = new BaseFSM<PlayerMotion>(motion);

            var idle = new FSMState<PlayerMotion>();
            idle.BindEnterAction(p => p.Motor.Idle());
            m_fsm.AddState("Idle", idle);
            m_fsm.SetDefault("Idle");

            var move = new FSMState<PlayerMotion>();
            move.BindUpdateAction(p => p.Motor.Move(p.Param.moveInput));
            m_fsm.AddState("Move", move);

            var jump = new FSMState<PlayerMotion>();
            jump.BindEnterAction(p => p.Motor.Jump());
            m_fsm.AddState("Jump", jump);

            var fall = new FSMState<PlayerMotion>();
            fall.BindEnterAction(p => p.Motor.Fall());
            m_fsm.AddState("Fall", fall);

            var roll = new FSMState<PlayerMotion>();
            roll.BindEnterAction(p => p.Motor.Roll(p.Param.moveInput));
            m_fsm.AddState("Roll", roll);

            var moveInput = new FSMCondition<PlayerMotion>(p => p.Param.moveInput.sqrMagnitude > 0.01f);
            var jumpInput = new FSMCondition<PlayerMotion>(p => p.Param.jump);
            var onGround = new FSMCondition<PlayerMotion>(p => p.Param.isOnGround);
            var velocityLess = new FSMCondition<PlayerMotion>(p => p.Model.GetComponent<Rigidbody>().velocity.y <= 0f);
            var jumpAnimEnd = new FSMCondition<PlayerMotion>(p => p.Param.IsAnimEnd("Jump"));
            var rollInput = new FSMCondition<PlayerMotion>(p => p.Param.roll);
            var rollAnimEnd = new FSMCondition<PlayerMotion>(p => p.Param.IsAnimEnd("Roll"));

            idle.AddConditon(jumpInput, "Jump");
            idle.AddConditon(rollInput, "Roll");
            idle.AddConditon(!onGround, "Fall");
            idle.AddConditon(moveInput, "Move");

            move.AddConditon(jumpInput, "Jump");
            move.AddConditon(!moveInput, "Idle");
            move.AddConditon(rollInput, "Roll");
            move.AddConditon(!onGround, "Fall");

            jump.AddConditon(onGround & velocityLess, "Idle");
            jump.AddConditon(jumpAnimEnd, "Fall");

            fall.AddConditon(onGround & velocityLess, "Idle");

            roll.AddConditon(rollAnimEnd, "Idle");
        }

        public void Update()
        {
            m_fsm.Update();
        }
    }
}
