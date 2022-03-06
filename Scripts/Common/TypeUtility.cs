using System;
using UnityEngine;

namespace IrisFenrir
{
    public class TypeUtility
    {
        public static T StringToEnum<T>(string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static Color StringToColor(string str)
        {
            if(str == null)
            {
                return Color.white;
            }

            var values = str.Substring(4).TrimStart('(').TrimEnd(')').Split(',');
            if(values.Length == 4)
            {
                return new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            }
            return Color.white;
        }

        public static Vector2 StringToVector2(string str)
        {
            if(str == null)
            {
                return Vector2.zero;
            }
            var values = str.TrimStart('(').TrimEnd(')').Split(',');
            if(values.Length == 2)
            {
                return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
            }
            return Vector2.zero;
        }

        public static Vector3 StringToVector3(string str)
        {
            if(str == null)
            {
                return Vector3.zero;
            }
            var values = str.TrimStart('(').TrimEnd(')').Split(',');
            if(values.Length == 3)
            {
                return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            }
            return Vector3.zero;
        }

        public static Vector4 StringToVector4(string str)
        {
            if (str == null)
            {
                return Vector4.zero;
            }
            var values = str.TrimStart('(').TrimEnd(')').Split(',');
            if (values.Length == 4)
            {
                return new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            }
            return Vector4.zero;
        }
    }
}
