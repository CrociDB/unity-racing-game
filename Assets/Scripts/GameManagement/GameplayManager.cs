using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Map;
using Game.Player;
using Game.UI;

namespace Game.GameManagement
{
    public class GameplayManager : MonoBehaviour
    {
        public MapController m_MapController;
        public PlayerMovement m_PlayerMovement;
        public GameplayUI m_GameplayUI;

        private bool m_GameRunning = false;
        private float m_Time;

        private void Start() 
        {
            Init();    
        }

        public void Init()
        {
            m_GameRunning = true;
            m_Time = 0.0f;
        }

        private void Update() 
        {
            if (m_GameRunning)
            {
                m_GameplayUI.SetSpeedNormalized(m_PlayerMovement.SpeedNormalized);
                m_GameplayUI.SetTime(m_Time);
                
                m_Time += Time.deltaTime;
            }
        }
    }
}
