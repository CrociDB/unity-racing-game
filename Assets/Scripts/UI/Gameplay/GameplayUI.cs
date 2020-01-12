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
        [Header("Panels")]
        public GameObject m_BoostPanel;
        public GameObject m_PauseScreenPanel;
        public GameObject m_EndGamePanel;
        public GameObject m_GameOver;

        [Header("HUD")]
        public Text m_Countdown;
        public Text m_Time;
        public Speedometer m_Speedometer;
        public Button m_PauseButton;
        public Image m_FlashFX;
        public Image m_BoostBase;

        [Header("End Game Panel")]
        public Image[] m_Stars;
        public Text m_TimeInfo;

        private List<Image> m_Boosts;

        private GameplayManager m_Gameplay;

        public void Init(GameplayManager gameplay)
        {
            m_Gameplay = gameplay;
        }

        public void SetSpeedNormalized(float speed)
        {
            m_Speedometer.SetSpeedNormalized(speed);
        }

        public void SetBoosts(int boosts)
        {
            m_Boosts = new List<Image>();
            for (int i = 0; i < boosts; i++)
            {
                var b = Instantiate(m_BoostBase);
                b.transform.SetParent(m_BoostBase.transform.parent);
                b.transform.localScale = Vector3.one;
                m_Boosts.Add(b);
            }

            Destroy(m_BoostBase.gameObject);
        }

        public void BurnBoost(float time)
        {
            if (m_Boosts.Count == 0) return;

            var b = m_Boosts[m_Boosts.Count - 1];
            b.DOColor(Color.red, 1.0f);
            b.transform.DOPunchScale(Vector3.one * 1.3f, 0.6f).SetUpdate(true).OnComplete(() => {
                m_Boosts.Remove(b);
                Destroy(b.gameObject);
            });

            m_Speedometer.BoostFor(time);
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
            m_BoostPanel.gameObject.SetActive(false);
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
            m_BoostPanel.gameObject.SetActive(true);
            m_Countdown.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(true);
            m_Speedometer.gameObject.SetActive(true);
        }

        public void Pause()
        {
            m_PauseButton.gameObject.SetActive(false);
            m_BoostPanel.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(false);
            m_Speedometer.gameObject.SetActive(false);

            m_Gameplay.PauseGame();
            m_PauseScreenPanel.SetActive(true);
            m_PauseScreenPanel.transform.localScale = Vector3.zero;
            m_PauseScreenPanel.transform.DOScale(Vector3.one, .4f).SetUpdate(true);
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
            m_BoostPanel.gameObject.SetActive(true);

            m_PauseScreenPanel.SetActive(false);
            m_Gameplay.UnpauseGame();
        }

        public void Restart()
        {
            GameManager.Instance.RestartLevel();
            m_Gameplay.UnpauseGame();
        }

        public void FlashScreen()
        {
            DOTween.Kill(m_FlashFX);

            var color = m_FlashFX.color;
            color.a = 0.0f;
            var target = color;
            target.a = 0.8f;
            m_FlashFX.DOColor(target, .07f).SetUpdate(true);
            m_FlashFX.DOColor(color, .3f).SetDelay(.075f).SetUpdate(true);
        }

        public void EndOfGame(int stars, float time)
        {
            StartCoroutine(EndOfGameRoutine(stars, time));
        }

        private IEnumerator EndOfGameRoutine(int stars, float time)
        {
            m_PauseButton.gameObject.SetActive(false);
            m_BoostPanel.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(false);
            m_Speedometer.gameObject.SetActive(false);

            int minutes = (int)Mathf.Floor(time / 60f);
            int seconds = (int)Mathf.Floor(time % 60f);
            int milliseconds = (int)Mathf.Floor((time % 1.0f) * 1000.0f);

            m_TimeInfo.text = String.Format("Your time: {0}' {1:00}\" {2:000}\nYour best time: {3}' {4:00}\" {5:000}", 
                                    minutes, seconds, milliseconds,
                                    minutes, seconds, milliseconds);

            m_PauseScreenPanel.SetActive(false);
            m_EndGamePanel.SetActive(true);
            m_EndGamePanel.transform.localScale = Vector3.zero;

            m_EndGamePanel.transform.DOScale(Vector3.one, .6f).SetUpdate(true);

            yield return new WaitForSecondsRealtime(.6f);

            for (int i = 0; i < stars; i++)
            {
                var targetColor = m_Stars[i].color;
                targetColor.a = 1.0f;
                m_Stars[i].DOColor(targetColor, .8f).SetUpdate(true);
                m_Stars[i].transform.DOPunchScale(Vector3.one * 1.2f, .2f).SetDelay(.2f).SetUpdate(true);

                yield return new WaitForSecondsRealtime(.8f);
            }
        }

        public void GameOver()
        {
            m_PauseButton.gameObject.SetActive(false);
            m_BoostPanel.gameObject.SetActive(false);
            m_Time.gameObject.SetActive(false);
            m_Speedometer.gameObject.SetActive(false);

            m_PauseScreenPanel.SetActive(false);
            m_GameOver.SetActive(true);
            m_GameOver.transform.localScale = Vector3.zero;

            m_GameOver.transform.DOScale(Vector3.one, .4f).SetUpdate(true);
        }
    }
}
