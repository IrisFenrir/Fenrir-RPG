using System.Collections.Generic;
using GraphVisualizer;
using IrisFenrir.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.MotionSystem
{
    public class PlayerAnim
    {
        private PlayableGraph m_graph;
        private Mixer m_mainMixer;
        private Dictionary<string, int> m_animStateIndex;
        private PlayerParam m_param;

        private BlendTree2D m_moveAnim;
        private AnimSelector m_rollAnim;

        public PlayerAnim(PlayerMotion motion, AnimSetting setting)
        {
            m_graph = PlayableGraph.Create();
            
            var root = new Root(m_graph);
            m_mainMixer = new Mixer(m_graph);
            m_animStateIndex = new Dictionary<string, int>();
            m_param = motion.Param;

            var idleAnim = new IdleAnim(m_graph, setting.GetParam("Idle"));
            AddAnimState("Idle", idleAnim);
            m_moveAnim = new BlendTree2D(m_graph, setting.GetParam("Move"));
            AddAnimState("Move", m_moveAnim);
            var jumpAnim = new AnimUnit(m_graph, setting.GetParam("Jump"));
            AddAnimState("Jump", jumpAnim);
            var fallAnim = new AnimUnit(m_graph, setting.GetParam("Fall"));
            AddAnimState("Fall", fallAnim);
            m_rollAnim = new AnimSelector(m_graph, setting.GetParam("Roll"));
            AddAnimState("Roll", m_rollAnim);

            motion.Param.AddAnim("Jump", jumpAnim);
            motion.Param.AddAnim("Roll", m_rollAnim);

            AnimHelper.SetOutput(m_graph, motion.Model.GetComponent<Animator>(), m_mainMixer);
            AnimHelper.Start(m_graph);
        }

        private void AddAnimState(string stateName, AnimBehaviour behaviour)
        {
            m_mainMixer.AddInput(behaviour);
            m_animStateIndex.Add(stateName, m_animStateIndex.Count);
        }

        public void Update()
        {
            GameLoop.Instance.jumpRemainTime = m_rollAnim.remainTime;
        }

        public void TransitionTo(string name)
        {
            if(m_animStateIndex.TryGetValue(name,out int index))
            {
                m_mainMixer.TransitionTo(index);
            }
        }

        public void SetMoveAnim(float x, float y)
        {
            if (m_moveAnim.enable)
            {
                m_moveAnim.SetPointer(x, y);
            }
        }

        public void SetRollAnim(float x, float y)
        {
            m_rollAnim.Select(y >= 0 ? 0 : 1);
        }

        public void Stop()
        {
            m_graph.Destroy();
        }
    }
}
