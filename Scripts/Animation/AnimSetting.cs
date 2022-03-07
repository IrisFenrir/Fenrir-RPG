using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace IrisFenrir.AnimationSystem
{
    [Serializable]
    public class AnimParam
    {
        public enum Type
        {
            Single,
            Group,
            InfoGroup,
            BlendClip
        }

        public string name = "Anim";
        public Type type = Type.Single;
        public float enterTime;
        [ShowIf("type",Type.Single)]
        public AnimationClip clip;
        [ShowIf("type", Type.Group)]
        public AnimationClip[] clipGroup;
        [ShowIf("type", Type.InfoGroup)]
        public AnimInfo[] infoGroup;
        [ShowIf("type", Type.BlendClip)]
        public BlendClip2D[] blendClip;
    }

    [Serializable]
    public class AnimInfo
    {
        [HorizontalGroup("Group", LabelWidth = 70)]
        public AnimationClip clip;
        [HorizontalGroup("Group")]
        public float enterTime;
    }

    [CreateAssetMenu(fileName = "New Anim Setting", menuName = "GuGame/Animation/Anim Setting")]
    public class AnimSetting : ScriptableObject
    {
        public List<AnimParam> animParams;

        public AnimParam GetParam(string name)
        {
            return animParams.Find(p => p.name == name);
        }
    }
}
