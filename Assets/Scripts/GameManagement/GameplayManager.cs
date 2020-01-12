﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Map;
using Game.Player;
using Game.UI;
using System;

namespace Game.GameManagement
{
    public class GameplayManager : MonoBehaviour
    {
        public MapController m_MapController;
        public PlayerMovement m_PlayerMovement;
        public GameplayUI m_GameplayUI;

        private float m_Time;

        private void Start() 
        {
            Init();    
        }

        public void Init()
        {
            m_Time = 0.0f;

            m_GameplayUI.Init(this);
        }

        private void Update() 
        {
            if (GameManager.Instance.Paused) return;
            
            m_GameplayUI.SetSpeedNormalized(m_PlayerMovement.SpeedNormalized);
            m_GameplayUI.SetTime(m_Time);

            m_Time += Time.deltaTime;
        }

        public void PauseGame()
        {
            GameManager.Instance.PauseGame();
        }

        public void UnpauseGame()
        {
            GameManager.Instance.UnpauseGame();
        }
        
    }
}
