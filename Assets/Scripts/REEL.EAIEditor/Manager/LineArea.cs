using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace REEL.EAIEditor
{
    [System.Serializable]
    public class DragInfo
    {
        public Vector2 topLeft = Vector2.zero;
        public float width = 0f;
        public float height = 0f;

        public void Reset()
        {
            topLeft = Vector2.zero;
            width = 0f;
            height = 0f;
        }

        public override string ToString()
        {
            return "topLeft: " + topLeft.ToString() + " width: " + width.ToString() + " height: " + height.ToString();
        }
    }

    public class LineArea : MonoBehaviour
    {
        public DragPane pane;
        public Image dragArea;
        public RectTransform topLeftTester;

        private RectTransform rectTransform;

        [SerializeField]
        private DragInfo dragInfo = new DragInfo();

        private void Awake()
        {
            rectTransform = dragArea.GetComponent<RectTransform>();

            pane.SubscribeDragEvent(OnDrag);
            pane.SubscribePointerUpEvent(OnPointerUp);
            pane.SubscribeBeginDragEvent(OnBeginDrag);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            rectTransform.position = dragInfo.topLeft = data.position;
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

            float width = data.position.x - rectTransform.position.x;
            float height = data.position.y - rectTransform.position.y;

            dragInfo.topLeft.x = width > 0f ? rectTransform.position.x : data.position.x;
            dragInfo.topLeft.y = height > 0f ? data.position.y : rectTransform.position.y;
            dragInfo.width = Mathf.Abs(width);
            dragInfo.height = Mathf.Abs(height);

            topLeftTester.position = dragInfo.topLeft;

            rectTransform.pivot = new Vector2(width > 0f ? 0f : 1f, height > 0f ? 0f : 1f);
            rectTransform.sizeDelta = new Vector2(dragInfo.width, dragInfo.height);

            //BlockDiagramManager.Instance.SetSelectionWithDragArea(dragArea.rectTransform);
            WorkspaceManager.Instance.SetBlockSelectionWithDragArea(dragInfo);
            WorkspaceManager.Instance.SetLineSelectedWithDragArea(dragInfo);
        }

        public  void OnPointerUp(PointerEventData data)
        {
            SelectLogic();
            SetAllDisabled();
            dragInfo.Reset();
        }
    }
}