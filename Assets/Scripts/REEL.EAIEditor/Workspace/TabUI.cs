using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class TabUI : MonoBehaviour
	{
        [SerializeField] private Sprite selectedSprite = null;
        [SerializeField] private Sprite unselectedSprite = null;
        [SerializeField] private Text projectNameText = null;

        private Image tabImage;

        [SerializeField] private bool isSelected = false;
        [SerializeField] private string tabName = string.Empty;

        private void Awake()
        {
            tabImage = GetComponent<Image>();
            projectNameText = GetComponentInChildren<Text>(true);
            ChangeState(false);
        }

        public void ChangeState(bool isSelected)
        {
            if (isSelected) tabImage.sprite = selectedSprite;
            else tabImage.sprite = unselectedSprite;

            this.isSelected = isSelected;
        }

        // 탭 이름(프로젝트 이름) 설정.
        public void SetTabName(string tabName)
        {
            this.tabName = tabName;
            if (projectNameText) projectNameText.text = tabName;
        }

        public string GetTabName {  get { return tabName; } }

        public Vector2 GetTabSize()
        {
            return tabImage.GetComponent<RectTransform>().sizeDelta;
        }

        public bool GetSelectedState {  get { return isSelected; } }
    }
}