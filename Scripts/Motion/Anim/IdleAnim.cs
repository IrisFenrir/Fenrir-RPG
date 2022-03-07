using IrisFenrir.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.MotionSystem
{
    public class IdleAnim : AnimBehaviour
    {
        private Mixer m_mixer;
        private float m_timeToNext;
        private bool m_isPlayDefault;
        private RandomSelector m_randomSelector;

        public IdleAnim(PlayableGraph graph, AnimParam param) : base(graph, param.enterTime)
        {
            m_mixer = new Mixer(graph);
            m_adapterPlayable.AddInput(m_mixer.GetAdapterPlayable(), 0, 1f);

            var clips = param.clipGroup;
            var defaultAnim = new AnimUnit(graph, clips[0], 0.5f);
            m_randomSelector = new RandomSelector(graph);
            for(int i=1; i<clips.Length; i++)
            {
                m_randomSelector.AddInput(clips[i], 0.5f);
            }
            m_mixer.AddInput(defaultAnim);
            m_mixer.AddInput(m_randomSelector);

            m_timeToNext = 10f;
            m_isPlayDefault = true;
            m_randomSelector.Select();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);

            if (!enable) return;
            m_timeToNext -= info.deltaTime;
            if(m_isPlayDefault)
            {
                if(m_timeToNext <= m_randomSelector.GetEnterTime())
                {
                    m_mixer.TransitionTo(1);
                    if(!m_mixer.isTransition)
                    {
                        m_timeToNext = m_randomSelector.GetAnimLength();
                        m_isPlayDefault = false;
                    }
                }
            }
            else
            {
                if(m_timeToNext <= 0.5f)
                {
                    m_mixer.TransitionTo(0);
                    if(!m_mixer.isTransition)
                    {
                        m_timeToNext = Random.Range(10f, 30f);
                        m_isPlayDefault = true;
                        m_randomSelector.Select();
                    }
                }
            }
        }

        public override void Enable()
        {
            base.Enable();

            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.Play();
            m_mixer.Enable();
        }

        public override void Disable()
        {
            base.Disable();

            m_adapterPlayable.Pause();
            m_mixer.Disable();
        }
    }
}
