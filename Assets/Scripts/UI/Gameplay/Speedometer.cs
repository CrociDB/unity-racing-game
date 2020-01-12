using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class Speedometer : MonoBehaviour
    {
        public RectTransform m_Pointer;

        private float m_NormalizedSpeed;

        public void SetSpeedNormalized(float speed)
        {
            m_NormalizedSpeed = speed;
        }

        private void FixedUpdate() 
        {
            var rot = m_Pointer.eulerAngles;
            var target = 90.0f - m_NormalizedSpeed * 180.0f;
            rot.z = target;
            m_Pointer.eulerAngles = rot;
        }
    }
}
