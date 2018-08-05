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

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            Debug.Log(rectTransform.position);
            Debug.Log(rectTransform.rect);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (beginDragEvent != null) beginDragEvent(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            float x = Mathf.Clamp(eventData.position.x, 0f, rectTransform.rect.width);
            float y = Mathf.Clamp(eventData.position.y, 0f, rectTransform.rect.height);
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