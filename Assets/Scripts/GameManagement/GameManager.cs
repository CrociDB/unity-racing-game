using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GameManagement
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _Instance;

        public static GameManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    var go = new GameObject("GameManager");
                    _Instance = go.AddComponent<GameManager>();
                    _Instance.Init();
                }

                return _Instance;
            }
        }

        public GameDescriptor m_GameDescriptor;

        private float m_FixedDeltaTime;
        private bool m_Paused;

        private LevelDescriptor m_CurrentLevel;

        public LevelDescriptor Currentlevel
        {
            get
            {
                if (m_CurrentLevel == null)
                {
                    var scene = SceneManager.GetActiveScene().name;
                    m_CurrentLevel = m_GameDescriptor.m_Levels.Where(l => l.m_Scene == scene).First();
                }

                return m_CurrentLevel;
            }
        }

        public bool Paused
        {
            get
            {
                return m_Paused;
            }
        }

        private void Init()
        {
            DontDestroyOnLoad(gameObject);
            m_FixedDeltaTime = Time.fixedDeltaTime;
            m_GameDescriptor = Resources.Load<GameDescriptor>("Descriptors/GameDescriptor");
        }

        public void LoadLevel(LevelDescriptor level)
        {
            m_CurrentLevel = level;
            UnpauseGame();
            SceneManager.LoadScene(level.m_Scene);
        }

        public void RestartLevel()
        {
            LoadLevel(m_CurrentLevel);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        // Time related operations
        public void PauseGame()
        {
            m_Paused = true;
            Time.timeScale = 0.0f;
            Time.fixedDeltaTime = 0.0f;
        }

        public void UnpauseGame()
        {
            Input.ResetInputAxes();
            m_Paused = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = m_FixedDeltaTime;
        }
    }
}
