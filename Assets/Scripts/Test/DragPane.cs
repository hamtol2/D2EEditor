using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace REEL.Test
{
    public class DragPane : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void eventDelegate(PointerEventData data);

        private event eventDelegate dragEvent;
        private event eventDelegate pointerUpEvent;
        private event eventDelegate beginDragEvent;

        private RectTransform rectTransform;

        private float rectX;
        private float rectY;
        private float halfWidth;
        private float halfHeight;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            rectX = rectTransform.position.x;
            rectY = rectTransform.position.y;
            halfWidth = rectTransform.rect.width * 0.5f;
            halfHeight = rectTransform.rect.height * 0.5f;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (beginDragEvent != null) beginDragEvent(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            float x = Mathf.Clamp(eventData.position.x, rectX - halfWidth, rectX + halfWidth);
            float y = Mathf.Clamp(eventData.position.y, rectY - halfHeight, rectY + halfHeight);
            eventData.position = new Vector2(x, y);

            if (dragEvent != null) dragEvent(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (pointerUpEvent != null) pointerUpEvent(eventData);
        }

        public void SubscribeDragEvent(eventDelegate dragDelegate)
        {
            dragEvent += dragDelegate;
        }

        public void SubscribePointerUpEvent(eventDelegate pointerUpDelegate)
        {
            pointerUpEvent += pointerUpDelegate;
        }

        public void SubscribeBeginDragEvent(eventDelegate beginDragDelegate)
        {
            beginDragEvent += beginDragDelegate;
        }
    }
}