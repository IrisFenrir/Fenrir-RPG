using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public abstract class AnimBehaviour
    {
        public bool enable { get; private set; }
        public float remainTime { get; protected set; }
        
        protected Playable m_adapterPlayable;
        protected float m_enterTime;
        protected float m_animLength;

        public AnimBehaviour(float enterTime = 0f) { m_enterTime = enterTime; }
        public AnimBehaviour(PlayableGraph graph, float enterTime = 0f)
        {
            m_adapterPlayable = ScriptPlayable<AnimAdapter>.Create(graph);
            AnimHelper.GetAdapter(m_adapterPlayable).Init(this);
            m_enterTime = enterTime;
            m_animLength = float.NaN;
        }

        public virtual void Enable() 
        {
            if (enable) return;
            enable = true;
            remainTime = GetAnimLength();
        }
        public virtual void Disable() 
        {
            if (!enable) return;
            enable = false; 
        }
        public virtual void Stop() { }
        
        public virtual void Execute(Playable playable, FrameData info)
        {
            if (!enable) return;
            remainTime = remainTime > 0f? remainTime - info.deltaTime: 0f;
        }

        public virtual void AddInput(Playable playable) { }
        public void AddInput(AnimBehaviour behaviour) 
        {
            AddInput(behaviour.GetAdapterPlayable());
        }

        public virtual Playable GetAdapterPlayable()
        {
            return m_adapterPlayable;
        }

        public virtual float GetEnterTime()
        {
            return m_enterTime;
        }
        public virtual float GetAnimLength()
        {
            return m_animLength;
        }

    }
}
