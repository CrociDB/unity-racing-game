using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Game.UI
{
    public class Speedometer : MonoBehaviour
    {
        public Image m_Marker;
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

        public void BoostFor(float time)
        {
            var color = m_Marker.color;
            m_Pointer.gameObject.SetActive(false);
            m_Marker.DOColor(Color.red, .2f).OnComplete(() => {
                m_Marker.DOColor(color, time).OnComplete(() => m_Pointer.gameObject.SetActive(true));
                m_Marker.transform.DOShakePosition(time, 1.5f, 10, 90);
            });
        }
    }
}
