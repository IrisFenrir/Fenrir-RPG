using System.Collections.Generic;
using IrisFenrir.AnimationSystem;
using UnityEngine;

namespace IrisFenrir.MotionSystem
{
    public class PlayerParam
    {
        public Vector2 moveInput;
        public bool run;
        public bool jump;
        public bool isOnGround;
        public Vector3 velocity;
        public bool roll;

        private Dictionary<string, AnimBehaviour> m_animMap;

        public void AddAnim(string name, AnimBehaviour anim)
        {
            if(m_animMap == null)
            {
                m_animMap = new Dictionary<string, AnimBehaviour>();
            }
            m_animMap.Add(name, anim);
        }
        public bool IsAnimEnd(string name, float threshold = 0.1f)
        {
            if(m_animMap.TryGetValue(name, out var anim))
            {
                return anim.enable && anim.remainTime <= threshold;
            }
            return false;
        }
    }
}
