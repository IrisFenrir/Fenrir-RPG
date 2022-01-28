using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IrisFenrir
{
    public class UIRaycaster
    {
        private static PointerEventData m_pointerData;
        private static EventSystem m_eventSystem;

        public static bool Raycast(GraphicRaycaster raycaster, out List<RaycastResult> results,
            Vector2 screenPos,EventSystem eventSystem = null)
        {
            if(eventSystem == null)
            {
                eventSystem = EventSystem.current;
            }
            if(m_eventSystem != eventSystem)
            {
                m_eventSystem = eventSystem;
                m_pointerData = new PointerEventData(m_eventSystem);
            }
            m_pointerData.position = screenPos;
            results = null;
            
            if(raycaster != null)
            {
                if(results == null)
                {
                    results = new List<RaycastResult>();
                }
                raycaster.Raycast(m_pointerData, results);
                if(results.Count > 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public static bool RaycastFromMouse(GraphicRaycaster raycaster, out List<RaycastResult> results,
            EventSystem eventSystem = null)
        {
            return Raycast(raycaster, out results, UnityEngine.Input.mousePosition, eventSystem);
        }

        public static bool RaycastWithClick(GraphicRaycaster raycaster, out List<RaycastResult> results,
            int mouseButton = 0,EventSystem eventSystem = null)
        {
            results = null;
            if(UnityEngine.Input.GetMouseButtonDown(mouseButton))
            {
                return RaycastFromMouse(raycaster, out results, eventSystem);
            }
            return false;
        }
    }
}
