// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PixelCrushers
{

    /// <summary>
    /// This script deselects the previous selectable when the pointer enters this one.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    [RequireComponent(typeof(Selectable))]
    public class DeselectPreviousOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IDeselectHandler, IEventSystemUser
    {

        [Tooltip("Do not deselect previous if previous is in this Exceptions list.")]
        [SerializeField] private List<GameObject> exceptions = new List<GameObject>();

        private UnityEngine.EventSystems.EventSystem m_eventSystem = null;
        public UnityEngine.EventSystems.EventSystem eventSystem
        {
            get
            {
                if (m_eventSystem == null) m_eventSystem = UnityEngine.EventSystems.EventSystem.current;
                return m_eventSystem;
            }
            set { m_eventSystem = value; }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventSystem == null || eventSystem.alreadySelecting) return;
            if (exceptions.Contains(eventSystem.currentSelectedGameObject)) return;
            eventSystem.SetSelectedGameObject(this.gameObject);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            GetComponent<Selectable>().OnPointerExit(null);
        }
    }
}