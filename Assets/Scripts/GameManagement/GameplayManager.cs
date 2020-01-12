using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Map;
using Game.Player;
using Game.UI;
using System;
using Game.Camera;

namespace Game.GameManagement
{
    public class GameplayManager : MonoBehaviour
    {
        public MapController m_MapController;
        public PlayerMovement m_PlayerMovement;
        public CameraFollower m_Camera;
        public GameplayUI m_GameplayUI;

        private bool m_GameRunning;
        private float m_Time;
        private int m_TouchedCheckpoints;

        private int m_Boosts;
        private float m_BoostCooldown = -1.0f;

        private void Start() 
        {
            Init();
        }

        public void Init()
        {
            m_Time = 0.0f;
            m_Boosts = 0;
            m_TouchedCheckpoints = 0;
            m_GameRunning = false;

            m_MapController.Init(this);
            m_GameplayUI.Init(this);
            m_GameplayUI.SetBoosts(GameManager.Instance.Currentlevel.m_Boosts);

            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            PauseGame();
            m_GameplayUI.StartCountdown();
            yield return new WaitForSecondsRealtime(4.0f);
            m_GameRunning = true;
            UnpauseGame();
        }

        private void Update()
        {
            if (GameManager.Instance.Paused) return;
            
            m_GameplayUI.SetSpeedNormalized(m_PlayerMovement.SpeedNormalized);
            m_GameplayUI.SetTime(m_Time);

            if (m_GameRunning) 
            {
                m_Time += Time.deltaTime;
                if (m_Time >= GameManager.Instance.Currentlevel.m_TimeLimit)
                {
                    GameOver();
                }

                RaycastHit playerHit;
                if (!Physics.Raycast(m_PlayerMovement.transform.position, -Vector3.up, out playerHit, Mathf.Infinity, 1) &&
                    m_PlayerMovement.transform.position.y <= -3.0f)
                {
                    GameOver();
                }

                m_BoostCooldown -= Time.deltaTime;
                if (Input.GetButtonDown("Fire1") && m_BoostCooldown < 0.0f)
                {
                    if (++m_Boosts <= GameManager.Instance.Currentlevel.m_Boosts)
                    {
                        Boost();
                    }
                }
            }
        }

        private void Boost()
        {
            m_BoostCooldown = 5.5f;
            m_Camera.BoostCamera(m_BoostCooldown);
            m_GameplayUI.BurnBoost(m_BoostCooldown);
            m_PlayerMovement.Boost();
        }

        public void PauseGame()
        {
            GameManager.Instance.PauseGame();
        }

        public void UnpauseGame()
        {
            GameManager.Instance.UnpauseGame();
        }

        private void GameOver()
        {
            PauseGame();
            m_GameplayUI.GameOver();
        }

        public void PlayerTouchedCheckpoint()
        {
            m_GameplayUI.FlashScreen();
            m_Camera.QuickBoost();

            if (++m_TouchedCheckpoints >= m_MapController.CheckpointTotal)
            {
                EndOfLevel();
            }
        }

        private void EndOfLevel()
        {
            StartCoroutine(EndOfLevelRoutine());
        }

        private IEnumerator EndOfLevelRoutine()
        {
            m_GameRunning = false;
            yield return new WaitForSecondsRealtime(.6f);
            PauseGame();

            var stars = GetRaceStars();

            var userData = GameManager.Instance.UserData.GetUserLevelData(GameManager.Instance.Currentlevel.name);
            if (userData == null || (userData != null && userData.m_Time > m_Time))
            {
                GameManager.Instance.UserData.SetUserLevelData(
                    GameManager.Instance.Currentlevel.name, 
                    new UserData.UserLevelData() {
                        m_Stars = stars, 
                        m_Time = m_Time });
                GameManager.Instance.SaveUserData();
            }

            m_GameplayUI.EndOfGame(stars, m_Time);
        }

        private int GetRaceStars()
        {
            var currentLevel = GameManager.Instance.Currentlevel;
            int stars = (m_Time < currentLevel.m_TimeByStar.x ? 
                            3 : 
                            (m_Time < currentLevel.m_TimeByStar.y ? 2 : 1));

            return stars;
        }
    }
}
