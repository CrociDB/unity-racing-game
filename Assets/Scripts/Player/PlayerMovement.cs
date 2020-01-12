using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameManagement;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody m_Body;
        public float m_MaxSpeed = 50.0f;
        public float m_Thrust = 1.0f;
        public float m_RotationSpeed = 4.0f;
        public float m_BoostSpeed = 10.0f;

        private float m_Boost;

        public ParticleSystem[] m_TireMarks;

        public float SpeedNormalized
        {
            get
            {
                return Mathf.Clamp01(m_Body.velocity.sqrMagnitude / (m_MaxSpeed * m_MaxSpeed));
            }
        }

        private void Start() 
        {
            m_Body = GetComponent<Rigidbody>();    
        }

        public void Update()
        {
            if (GameManager.Instance.Paused) return;

            m_Boost *= 0.99f;
            var maxSpeed = m_MaxSpeed + m_Boost;
            var thrust = m_Thrust + m_Boost * 0.5f;
            
            if (m_Boost >= 0.1f)
                Debug.Log("Added boost. Total speed: " + maxSpeed);

            m_Body.AddForce(transform.forward * thrust * Time.deltaTime * 60f, ForceMode.Acceleration);

            var velocity = m_Body.velocity;
            var speedNormalized = Mathf.Clamp(velocity.sqrMagnitude / (m_MaxSpeed * m_MaxSpeed), 0.2f, 1.0f);

            var turning = Input.GetAxis("Horizontal") * speedNormalized;

            foreach(var tm in m_TireMarks)
            {
                var diff = 1.0f - Vector3.Dot(velocity.normalized, transform.forward);
                tm.Emit((int)(Mathf.Min(Mathf.Abs(diff) * 8.0f, 8.0f)));
            }

            transform.Rotate(transform.up * turning * m_RotationSpeed * Time.deltaTime * 60f);
            m_Body.AddForce(-velocity * Mathf.Abs(turning), ForceMode.Acceleration);

            if (velocity.sqrMagnitude >= maxSpeed * maxSpeed)
            {
                m_Body.AddForce(-velocity * .7f, ForceMode.Acceleration);
            }
        }

        public void Boost()
        {
            m_Boost = m_BoostSpeed;
        }
    }
}
