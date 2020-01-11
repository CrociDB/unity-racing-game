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
        public Text m_UserStars;
        public Text m_UserTime;

        [HideInInspector]
        public LevelDescriptor m_LevelDescriptor;

        public void Build(LevelDescriptor levelDescriptor)
        {
            m_LevelDescriptor = levelDescriptor;

            m_Title.text = m_LevelDescriptor.m_Name;
            m_Difficulty.text = m_LevelDescriptor.m_Name;
            m_UserStars.text = "Stars";
            m_UserTime.text = "UserTime";

            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.LoadLevel(levelDescriptor));
        }
    }
}
