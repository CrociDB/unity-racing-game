using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using Game.GameManagement;

namespace Game.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public GameObject m_PauseScreenPanel;
        public Text m_Countdown;
        public Text m_Time;
        public Speedometer m_Speedometer;
        public Button m_PauseButton;
        
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

        public void StartCountdown()
        {
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            m_PauseButton.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(false);
            m_Speedometer.gameObject.SetActive(false);
            m_Countdown.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                m_Countdown.transform.localScale = Vector3.zero;
                m_Countdown.text = i.ToString();
                m_Countdown.transform.DOScale(Vector3.one * 1.4f, 0.95f).SetUpdate(true);
                yield return new WaitForSecondsRealtime(1.0f);
            }

            m_Countdown.transform.localScale = Vector3.zero;
            m_Countdown.text = "GO!";
            m_Countdown.transform.DOScale(Vector3.one * 1.6f, 1.0f).SetUpdate(true);
            yield return new WaitForSecondsRealtime(1.0f);

            m_PauseButton.gameObject.SetActive(true);
            m_Countdown.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(true);
            m_Speedometer.gameObject.SetActive(true);
        }

        public void Pause()
        {
            m_PauseButton.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(false);
            m_Speedometer.gameObject.SetActive(false);

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
            m_PauseButton.gameObject.SetActive(true);
            m_Time.gameObject.SetActive(true);
            m_Speedometer.gameObject.SetActive(true);

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
