using System.Collections;
using System.Collections.Generic;
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
            m_Body.AddForce(transform.forward * m_Thrust, ForceMode.Acceleration);

            var velocity = m_Body.velocity;
            var speedNormalized = Mathf.Clamp(velocity.sqrMagnitude / (m_MaxSpeed * m_MaxSpeed), 0.2f, 1.0f);

            var turning = Input.GetAxis("Horizontal") * speedNormalized;

            foreach(var tm in m_TireMarks)
            {
                var diff = 1.0f - Vector3.Dot(velocity.normalized, transform.forward);
                tm.Emit((int)(Mathf.Min(Mathf.Abs(diff) * 8.0f, 8.0f)));
            }

            transform.Rotate(transform.up * turning * m_RotationSpeed);
            m_Body.AddForce(-velocity * Mathf.Abs(turning), ForceMode.Acceleration);

            if (velocity.sqrMagnitude >= m_MaxSpeed * m_MaxSpeed)
            {
                m_Body.AddForce(-velocity * .7f, ForceMode.Acceleration);
            }
        }
    }
}
