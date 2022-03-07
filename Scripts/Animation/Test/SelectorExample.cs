using UnityEngine;
using UnityEngine.Playables;

namespace IrisFenrir.AnimationSystem
{
    public class SelectorExample : MonoBehaviour
    {
        public AnimationClip[] clips;
        public int index;
        public float remainTime;

        private PlayableGraph graph;
        private AnimSelector selector;

        private void Start()
        {
            graph = PlayableGraph.Create();

            selector = new AnimSelector(graph);
            foreach (var clip in clips)
            {
                selector.AddInput(clip, 0.2f);
            }

            AnimHelper.SetOutput(graph, GetComponent<Animator>(), selector);
            selector.Select();
            AnimHelper.Start(graph);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(selector.enable)
                {
                    selector.Disable();
                }
                else
                {
                    selector.Select();
                    selector.Enable();
                }
            }

            remainTime = selector.remainTime;
        }

        private int Select()
        {
            return index++ % clips.Length;
        }

        private void OnDisable()
        {
            graph.Destroy();
        }
    }
}
