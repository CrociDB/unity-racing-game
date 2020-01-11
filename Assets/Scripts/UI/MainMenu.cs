using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Game.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        public RectTransform m_TitlePanel;
        public RectTransform m_SelectionPanel;

        private Canvas m_Canvas;

        private void Awake() 
        {
            m_Canvas = GetComponent<Canvas>();

            m_TitlePanel.gameObject.SetActive(true);   
            m_SelectionPanel.gameObject.SetActive(true);
        }

        public void ShowLevelSelection()
        {
            m_TitlePanel.DOAnchorPos(m_TitlePanel.anchoredPosition + Vector2.left * 800.0f, .2f);
            m_SelectionPanel.DOAnchorPos(m_SelectionPanel.anchoredPosition + Vector2.left * 800.0f, .2f);
        }

        public void ShowMainMenu()
        {
            m_TitlePanel.DOAnchorPos(m_TitlePanel.anchoredPosition + Vector2.right * 800.0f, .2f);
            m_SelectionPanel.DOAnchorPos(m_SelectionPanel.anchoredPosition + Vector2.right * 800.0f, .2f);
        }
    }
}
