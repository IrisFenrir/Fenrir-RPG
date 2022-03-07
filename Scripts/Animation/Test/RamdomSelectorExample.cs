using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class RamdomSelectorExample : MonoBehaviour
    {
        public bool isTransition;
        public float remainingTime;
        public AnimationClip[] clips;

        private PlayableGraph m_graph;
        private Mixer mixer;
        private RandomSelector randomSelector;

        private void Start()
        {
            m_graph = PlayableGraph.Create();

            var anim1 = new AnimUnit(m_graph, clips[0], 0.5f);
            mixer = new Mixer(m_graph);
            randomSelector = new RandomSelector(m_graph);
            randomSelector.AddInput(clips[1], 0.5f);
            randomSelector.AddInput(clips[2], 0.5f);
            randomSelector.AddInput(clips[3], 0.5f);
            randomSelector.AddInput(clips[4], 0.5f);
            mixer.AddInput(anim1);
            mixer.AddInput(randomSelector);

            AnimHelper.SetOutput(m_graph, GetComponent<Animator>(), mixer);
            AnimHelper.Start(m_graph);
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                randomSelector.Select();
                mixer.TransitionTo(1);
            }
            isTransition = mixer.isTransition;
            remainingTime = randomSelector.remainTime;
            if(!mixer.isTransition && randomSelector.remainTime <= 0.5f)
            {
                mixer.TransitionTo(0);
            }
        }

        private void OnDisable()
        {
            m_graph.Destroy();
        }
    }
}