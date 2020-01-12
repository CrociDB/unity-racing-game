using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelSelectionItem : MonoBehaviour
    {
        public Text m_Title;
        public Text m_Difficulty;
        public Image[] m_UserStars;
        public Text m_UserTime;

        [HideInInspector]
        public LevelDescriptor m_LevelDescriptor;

        public void Build(LevelDescriptor levelDescriptor)
        {
            m_LevelDescriptor = levelDescriptor;

            m_Title.text = m_LevelDescriptor.m_Title;
            m_Difficulty.text = m_LevelDescriptor.m_Difficulty.ToString();
            m_UserTime.text = "---";

            int stars = 2;
            for (int i = 0; i < stars; i++)
            {
                var s = m_UserStars[i];
                var color = s.color;
                color.a = 1.0f;
                s.color = color;
            }

            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.LoadLevel(levelDescriptor));
        }
    }
}
