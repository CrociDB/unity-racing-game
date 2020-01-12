using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Game.Camera
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform m_FollowObject;
        public float m_DistanceToObject;
        public float m_Height;
        public float m_SmoothMovement = 0.3f;
        public float m_RotationLerpRate = 3.0f;

        private Vector3 m_FollowVelocity;

        private UnityEngine.Camera m_Camera;

        private void Awake() 
        {
            m_Camera = GetComponentInChildren<UnityEngine.Camera>();
        }

        void FixedUpdate()
        {
            var position = m_FollowObject.position - m_FollowObject.forward * m_DistanceToObject + Vector3.up * m_Height;
            transform.position = Vector3.SmoothDamp(transform.position, position, ref m_FollowVelocity, m_SmoothMovement);

            transform.forward = Vector3.Lerp(transform.forward, m_FollowObject.forward, m_RotationLerpRate * Time.deltaTime);  
        }

        public void QuickBoost()
        {
            m_Camera.transform.DOShakeRotation(.2f, 40, 100);
        }

        public void BoostCamera(float time)
        {
            var fov = m_Camera.fieldOfView;
            m_Camera.transform.DOShakePosition(time, .2f, 60, 90);
            m_Camera.DOFieldOfView(35f, .1f).OnComplete(() => m_Camera.DOFieldOfView(fov, time * .6f).SetDelay(time * .4f - .1f));
        }
    }
}
