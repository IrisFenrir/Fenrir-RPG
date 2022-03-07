using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class BlendTreeExample : MonoBehaviour
    {
        public Vector2 pointer;
        public BlendClip2D[] clips;
        public ComputeShader shader;

        private BlendTree2D m_blend;
        private PlayableGraph m_graph;

        private void Start()
        {
            m_graph = PlayableGraph.Create();

            m_blend = new BlendTree2D(m_graph, clips);

            AnimHelper.SetOutput(m_graph, GetComponent<Animator>(), m_blend);
            AnimHelper.Start(m_graph);
        }

        private void Update()
        {
            m_blend.SetPointer(pointer);
        }

        private void OnDisable()
        {
            m_graph.Destroy();
        }
    }
}