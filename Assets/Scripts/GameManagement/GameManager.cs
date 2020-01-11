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

        private void Init()
        {
            DontDestroyOnLoad(gameObject);
            m_GameDescriptor = Resources.Load<GameDescriptor>("Descriptors/GameDescriptor");
        }

        public void LoadLevel(LevelDescriptor level)
        {
            SceneManager.LoadScene(level.m_Scene);
        }
    }
}
