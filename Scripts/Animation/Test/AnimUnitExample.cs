using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class AnimUnitExample : MonoBehaviour
    {
        public AnimationClip clip;

        private PlayableGraph m_graph;
        private AnimUnit m_animUnit;

        // Use this for initialization
        void Start()
        {
            m_graph = PlayableGraph.Create();
            m_animUnit = new AnimUnit(m_graph, clip);
            AnimHelper.SetOutput(m_graph, GetComponent<Animator>(), m_animUnit);
            AnimHelper.Start(m_graph);
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                if (m_animUnit.enable)
                    m_animUnit.Disable();
                else
                    m_animUnit.Enable();
            }
        }

        private void OnDisable()
        {
            m_graph.Destroy();
        }
    }
}