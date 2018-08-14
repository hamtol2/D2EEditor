using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
    {
        public delegate void OnChanged();
        private event OnChanged onChanged;

        public delegate void OnUpdate(PointerEventData eventData);
        private event OnUpdate onUpdate;

        protected RectTransform refRectTransform;
        protected Vector3 originPosition;

        private bool canDrag = true;
        private Vector3 dragOffset;

        void Awake()
        {
            refRectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            originPosition = refRectTransform.position;

            BlockDiagramManager.Instance.SetDragOffset(eventData.position);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;

            BlockDiagramManager.Instance.BlockDrag(eventData);
        }

        public void SetDragOffset(Vector3 pointerPosition)
        {
            dragOffset = refRectTransform.position - pointerPosition;
        }

        private void ExecuteOnChanged()
        {
            if (onChanged != null) onChanged();
        }

        private void ExecuteOnUpdate(PointerEventData eventData)
        {
            if (onUpdate != null) onUpdate(eventData);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetEnableDrag(true);
        }

        protected void SetToOrigin()
        {
            refRectTransform.position = originPosition;
        }

        //protected void ChangePosition(Vector3 newPos)
        public void ChangePosition(PointerEventData eventData)
        {
            // Test.
            refRectTransform.position = (Vector3)eventData.position + dragOffset;
            
            //refRectTransform.position = newPos;

            ExecuteOnChanged();
            ExecuteOnUpdate(eventData);
        }

        public void SetEnableDrag(bool enable)
        {
            canDrag = enable;
        }

        public void SubscribeOnChanged(OnChanged onChanged)
        {
            this.onChanged += onChanged;
        }

        public void UnsubscribeOnChanged(OnChanged onChanged)
        {
            this.onChanged -= onChanged;
        }

        public void SubscribeOnUpdate(OnUpdate onUpdate)
        {
            this.onUpdate += onUpdate;
        }

        public void UnsubscribeOnUpdate(OnUpdate onUpdate)
        {
            this.onUpdate -= onUpdate;
        }
    }
}