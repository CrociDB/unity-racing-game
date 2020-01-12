using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public GameObject m_PauseScreenPanel;
        public Text m_Time;
        public Speedometer m_Speedometer;
        
        private GameplayManager m_Gameplay;

        public void Init(GameplayManager gameplay)
        {
            m_Gameplay = gameplay;
        }

        public void SetSpeedNormalized(float speed)
        {
            m_Speedometer.SetSpeedNormalized(speed);
        }

        public void SetTime(float time)
        {
            int minutes = (int)Mathf.Floor(time / 60f);
            int seconds = (int)Mathf.Floor(time % 60f);
            int milliseconds = (int)Mathf.Floor((time % 1.0f) * 1000.0f);
            m_Time.text = string.Format("{0}'\t{1:00}\"\t{2:000}", minutes, seconds, milliseconds);
        }

        public void Pause()
        {
            m_Gameplay.PauseGame();
            m_PauseScreenPanel.SetActive(true);
        }

        public void ExitToMenu()
        {
            GameManager.Instance.LoadMainMenu();
            m_Gameplay.UnpauseGame();
        }

        public void Resume()
        {
            m_PauseScreenPanel.SetActive(false);
            m_Gameplay.UnpauseGame();
        }

        public void Restart()
        {
            GameManager.Instance.RestartLevel();
            m_Gameplay.UnpauseGame();
        }
    }
}
