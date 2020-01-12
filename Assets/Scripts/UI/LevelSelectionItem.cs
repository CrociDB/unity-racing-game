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

            var userData = GameManager.Instance.UserData.GetUserLevelData(m_LevelDescriptor.name);

            if (userData != null)
            {
                for (int i = 0; i < userData.m_Stars; i++)
                {
                    var s = m_UserStars[i];
                    var color = s.color;
                    color.a = 1.0f;
                    s.color = color;
                }

                int minutes = (int)Mathf.Floor(userData.m_Time / 60f);
                int seconds = (int)Mathf.Floor(userData.m_Time % 60f);
                int milliseconds = (int)Mathf.Floor((userData.m_Time % 1.0f) * 1000.0f);

                m_UserTime.text = string.Format("{0}'\t{1:00}\"\t{2:000}", minutes, seconds, milliseconds);
            }


            GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.LoadLevel(levelDescriptor));
        }
    }
}
