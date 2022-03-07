using IrisFenrir.InputSystem;

namespace IrisFenrir.MotionSystem
{
    public class PlayerInput
    {
        private PlayerParam m_param;

        public PlayerInput(PlayerMotion motion)
        {
            m_param = motion.Param;
        }

        public void Enable(bool enable)
        {
            InputManager.Enable<AxisKey>("Move", enable);
            InputManager.Enable<TapKey>("Jump", enable);
            InputManager.Enable<TapKey>("Roll", enable);
        }

        public void Update()
        {
            if (m_param == null) return;

            m_param.moveInput = InputManager.GetAxis2D("Move");
            if(InputManager.GetTap("Run"))
            {
                m_param.run = !m_param.run;
            }
            m_param.jump = InputManager.GetTap("Jump");
            m_param.roll = InputManager.GetTap("Roll");
        }
    }
}
