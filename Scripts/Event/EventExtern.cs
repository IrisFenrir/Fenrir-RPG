using IrisFenrir.InputSystem;

namespace IrisFenrir.EventSystem
{
    public static class EventExtern
    {
        public static bool CreateTapKeyEvent(this EventManager mgr, string name , string parent = "Root")
        {
            var e = EventManager.Instance.CreateEvent(name, parent);
            if (e == null) return false;
            e.Register(() => InputManager.GetTap(name));
            return true;
        }
    }
}
