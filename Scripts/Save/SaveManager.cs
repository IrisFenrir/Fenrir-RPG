using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace IrisFenrir.SaveSystem
{
    public class SaveManager
    {
        // 通过json将对象保存、加载
        public static void SaveWithJson(object obj,string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(path);
            var json = JsonUtility.ToJson(obj);
            formatter.Serialize(file, json);
            file.Close();
        }
        public static bool LoadWithJson<T>(ref T obj,string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), obj);
                file.Close();
                return true;
            }
            return false;
        }
    }
}
