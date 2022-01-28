using UnityEngine;

namespace IrisFenrir.Input
{
    [System.Serializable]
    // ��������࣬���еĹ��ܼ����Ӵ�����
    public abstract class VirtualKey
    {
        // ������ͬ���ͼ���Ҫ����
        public string name;
        // �Ƿ�����
        [SerializeField, HideInInspector]
        protected bool enable;

        // ��ʼ����Ҫ����帳ֵ������ʹ�ù��캯��������ʹ��ǰ����Init��ʼ��
        public virtual void Init() 
        {
            enable = true;
        }
        // ����/���������
        public virtual void SetEnable(bool _enable)
        {
            enable = _enable;
        }
        // ����
        public abstract void Update();
        // ���ü�λ
        public abstract void SetKeyCode(params KeyCode[] keyCodes);
    }
}

