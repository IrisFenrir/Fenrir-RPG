using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class MixerExample : MonoBehaviour
    {
        public AnimationClip[] clips;
        public bool canInteruptTransition = true;
        public float totalWeight;

        private PlayableGraph m_graph;
        private Mixer m_mixer;

        private void Start()
        {
            m_graph = PlayableGraph.Create();
            var animUnit1 = new AnimUnit(m_graph, clips[0], 0.1f);
            var animUnit2 = new AnimUnit(m_graph, clips[1], 0.1f);
            var animUnit3 = new AnimUnit(m_graph, clips[2], 0.1f);
            m_mixer = new Mixer(m_graph);
            m_mixer.AddInput(animUnit1);
            m_mixer.AddInput(animUnit2);
            m_mixer.AddInput(animUnit3);

            AnimHelper.SetOutput(m_graph, GetComponent<Animator>(), m_mixer);
            AnimHelper.Start(m_graph);
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKey(KeyCode.X))
            {
                m_mixer.TransitionTo(0);
            }
            else if(UnityEngine.Input.GetKey(KeyCode.C))
            {
                m_mixer.TransitionTo(1);
            }
            else if (UnityEngine.Input.GetKey(KeyCode.V))
            {
                m_mixer.TransitionTo(2);
            }

            totalWeight = 0f;
            for (int i = 0; i < clips.Length; i++)
            {
                totalWeight += m_mixer.GetWeight(i);
            }
            if (totalWeight > 1f) Debug.Log($"权重超出1: {totalWeight}");
        }

        private void OnDisable()
        {
            m_graph.Destroy();
        }
    }
}
