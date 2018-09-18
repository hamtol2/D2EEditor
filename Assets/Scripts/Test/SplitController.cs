using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace REEL.Test
{
	public class SplitController : MonoBehaviour
	{
        public UISplitBar verticalSplitBar;
        public UISplitBar horizontalSplitBar;
        public EventSystem eventSystem;
        public GraphicRaycaster raycaster;
        PointerEventData data;
        public bool isSelected = false;
        public UISplitBar selectedSplitBar;

        RectTransform rectTransform;
        private float offset = 0f;
        private float rectX;
        private float rectY;
        private float halfWidth = 0f;
        private float halfHeight = 0f;

        private void Awake()
        {
            data = new PointerEventData(eventSystem);
            rectTransform = GetComponent<RectTransform>();
            rectX = rectTransform.position.x;
            rectY = rectTransform.position.y;
            halfWidth = rectTransform.rect.width * 0.5f;
            halfHeight = rectTransform.rect.height * 0.5f;
            //Debug.Log(GetComponent<RectTransform>().rect);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                data.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(data, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Player"))
                    {
                        verticalSplitBar = result.gameObject.GetComponent<UISplitBar>();
                        isSelected = true;
                        offset = verticalSplitBar.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                        //offset = selectedSplitBar.sizeDelta.x * 0.5f;
                    }
                    else if (result.gameObject.CompareTag("Finish"))
                    {
                        horizontalSplitBar = result.gameObject.GetComponent<UISplitBar>();
                        isSelected = true;
                        offset = horizontalSplitBar.GetComponent<RectTransform>().sizeDelta.y * 0.5f;
                    }
                }
            }

            if (Input.GetMouseButton(0) && isSelected)
            {
                if (verticalSplitBar)
                {
                    float xPos = Input.mousePosition.x;
                    xPos = Mathf.Clamp(xPos, rectX - halfWidth + offset, rectX + halfWidth - offset);
                    verticalSplitBar.PositionUpdate(new Vector2(xPos, verticalSplitBar.GetComponent<RectTransform>().position.y));
                }
                else if (horizontalSplitBar)
                {
                    float yPos = Input.mousePosition.y;
                    yPos = Mathf.Clamp(yPos, rectY - halfHeight + offset, rectY + halfHeight - offset);
                    horizontalSplitBar.PositionUpdate(new Vector2(horizontalSplitBar.GetComponent<RectTransform>().position.x, yPos));
                }
                
                //Debug.Log((-halfWidth + offset) + " : " + (halfWidth - offset) + ", " + xPos);
                //selectedTransform.position = new Vector2(xPos, selectedTransform.position.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isSelected = false;
                selectedSplitBar = null;
                verticalSplitBar = horizontalSplitBar = null;
            }
        }
    }
}
