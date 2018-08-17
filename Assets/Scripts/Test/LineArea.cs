using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    public class LineArea : MonoBehaviour
    {
        public DragPane pane;
        public Image dragArea;

        private RectTransform rectTransform;
        private Vector2 startPos;
        private Vector2 endPos;

        private void Awake()
        {
            rectTransform = dragArea.GetComponent<RectTransform>();

            pane.SubscribeDragEvent(OnDrag);
            pane.SubscribePointerUpEvent(OnPointerUp);
            pane.SubscribeBeginDragEvent(OnBeginDrag);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            rectTransform.position = startPos = data.position;
        }

        void SelectLogic()
        {

        }

        void SetAllEnabled()
        {
            if (!dragArea.gameObject.activeSelf) dragArea.gameObject.SetActive(true);
        }

        void SetAllDisabled()
        {
            if (dragArea.gameObject.activeSelf) dragArea.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData data)
        {
            SetAllEnabled();

            float width = data.position.x - dragArea.transform.position.x;
            float height = data.position.y - dragArea.transform.position.y;
            rectTransform.pivot = new Vector2(width > 0f ? 0f : 1f, height > 0f ? 0f : 1f);
            rectTransform.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

            BlockDiagramManager.Instance.SetSelectionWithDragArea(dragArea.rectTransform);
        }

        public  void OnPointerUp(PointerEventData data)
        {
            endPos = data.position;
            SelectLogic();
            SetAllDisabled();
        }
    }
}