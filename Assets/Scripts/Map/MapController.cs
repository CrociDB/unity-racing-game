using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.GameManagement;
using System;

namespace Game.Map
{
    public class MapController : MonoBehaviour
    {
        public Checkpoint m_CheckpointPrefab;

        private List<Checkpoint> m_Checkpoints;
        private GameplayManager m_Gameplay;
        private CircuitGenerator m_Circuit;

        public void Init(GameplayManager gameplay)
        {
            m_Gameplay = gameplay;
            m_Circuit = GetComponentInChildren<CircuitGenerator>();

            InitCheckpoints();
        }

        private void InitCheckpoints()
        {
            m_Checkpoints = new List<Checkpoint>();

            foreach (var cp in m_Circuit.GetCheckpoints())
            {
                var checkpoint = Instantiate(m_CheckpointPrefab);
                checkpoint.transform.SetParent(transform);
                checkpoint.transform.position = cp.Item1 + Vector3.up * 3.5f;
                checkpoint.transform.localScale = new Vector3(m_Circuit.m_Width * 2.0f, 7.0f, 1.0f);
                checkpoint.transform.forward = cp.Item2;

                checkpoint.OnPlayerTouched += PlayerTouchedCheckpoint;

                m_Checkpoints.Add(checkpoint);
            }

            m_Checkpoints[0].SetFinalCheckpoint();
        }

        private void PlayerTouchedCheckpoint(Checkpoint checkpoint)
        {
            m_Gameplay.PlayerTouchedCheckpoint();
            m_Checkpoints.Remove(checkpoint);
            Destroy(checkpoint.gameObject);
        }
    }
}
