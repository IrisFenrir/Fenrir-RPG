using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class AnimAdapter : PlayableBehaviour
    {
        private AnimBehaviour m_behaviour;

        public void Init(AnimBehaviour behaviour)
        {
            m_behaviour = behaviour;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            m_behaviour?.Execute(playable, info);
        }

        public void Enable()
        {
            m_behaviour?.Enable();
        }

        public void Disable()
        {
            m_behaviour?.Disable();
        }

        public T GetAnimBehaviour<T>() where T:AnimBehaviour
        {
            return m_behaviour as T;
        }

        public float GetAnimEnterTime()
        {
            return m_behaviour.GetEnterTime();
        }

        public override void OnGraphStop(Playable playable)
        {
            m_behaviour?.Stop();
        }
    }
}
