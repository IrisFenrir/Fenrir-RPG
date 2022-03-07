using UnityEngine;

namespace IrisFenrir.MotionSystem
{
    public class PlayerMotor
    {
        private PlayerParam m_param;

        private Transform m_model;
        private PlayerAnim m_anim;

        private Rigidbody m_rigidBody;
        private float m_walkSpeed;
        private float m_rotateSpeed;
        private float m_runSpeed;
        private float m_jumpForce;
        private float m_rollForce;

        private float m_animMultiply;
        private float m_speedMultiply;

        public PlayerMotor(PlayerMotion motion)
        {
            m_param = motion.Param;

            m_model = motion.Model;
            m_anim = motion.Anim;

            m_rigidBody = m_model.GetComponent<Rigidbody>();
            m_walkSpeed = GameLoop.Instance.walkSpeed;
            m_rotateSpeed = GameLoop.Instance.rotateSpeed;
            m_runSpeed = GameLoop.Instance.runSpeed;
            m_jumpForce = GameLoop.Instance.jumpForce;
            m_rollForce = GameLoop.Instance.rollForce;

            m_animMultiply = 1f;
            m_speedMultiply = 1f;
        }

        public void Idle()
        {
            m_anim.TransitionTo("Idle");
        }

        public void Move(Vector2 input)
        {
            // Anim
            m_anim.TransitionTo("Move");
            if(m_param.run && input.y > 0)
            {
                m_animMultiply = Mathf.Clamp(UpdateMultiply(m_animMultiply, 5f), 1f, 2f);
                m_speedMultiply = Mathf.Clamp(UpdateMultiply(m_speedMultiply, 5f), m_walkSpeed, m_runSpeed);
            }
            else
            {
                m_animMultiply = Mathf.Clamp(UpdateMultiply(m_animMultiply, -5f), 1f, 2f);
                m_speedMultiply = Mathf.Clamp(UpdateMultiply(m_speedMultiply, -5f), m_walkSpeed, m_runSpeed);
            }
            m_anim.SetMoveAnim(input.x, input.y * m_animMultiply);
            // Action
            //m_model.Translate(Vector3.forward * input.y * m_speedMultiply * Time.deltaTime);
            m_rigidBody.velocity = new Vector3(m_model.forward.x * input.y * m_speedMultiply, m_rigidBody.velocity.y, m_model.forward.z * input.y * m_speedMultiply);
            m_param.velocity = m_rigidBody.velocity;
            m_model.Rotate(Vector3.up * input.x * m_rotateSpeed * Time.deltaTime);
        }

        public void Jump()
        {
            m_anim.TransitionTo("Jump");
            m_rigidBody.velocity += Vector3.up * m_jumpForce;
        }

        public void Fall()
        {
            m_anim.TransitionTo("Fall");
        }

        public void Roll(Vector2 input)
        {
            m_anim.SetRollAnim(input.x, input.y);
            m_anim.TransitionTo("Roll");
            m_rigidBody.velocity += m_model.forward * m_rollForce * (input.y >= 0 ? 1 : -1);
        }

        private float UpdateMultiply(float value, float speed)
        {
            return value + speed * Time.deltaTime;
        }
    }
}
