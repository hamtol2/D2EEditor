using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class GraphItem : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
    {
        public NodeType nodeType;

        [SerializeField]
        private EditorManager.ETargetMenuType targetMenuType;

        private RectTransform refRectTransform;
        private Vector3 originPosition;

        protected MenuPopup targetPopup;

        [SerializeField]
        private object data;

        protected void Awake()
        {
            Init();
        }

        private void Init()
        {
            refRectTransform = GetComponent<RectTransform>();
            targetPopup = EditorManager.Instance.GetTargetMenuObject(targetMenuType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            originPosition = refRectTransform.position;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IfMoved) return;

            if (eventData.clickCount == 2)
            {
                if (targetPopup != null) targetPopup.ShowPopup(this);
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (IfMoved) return;
            //if (targetPopup != null) targetPopup.ShowPopup(this);
        }

        public virtual void SetItemData(object data)
        {
            this.data = data;
            //Debug.Log("item name: " + data);
        }

        public virtual object GetItemData()
        {
            return this.data;
        }

        bool IfMoved
        {
            get { return refRectTransform.position != originPosition; }
        }
    }
}