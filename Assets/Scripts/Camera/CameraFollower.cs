using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform m_FollowObject;
    public float m_DistanceToObject;
    public float m_Height;
    public float m_SmoothMovement = 0.3f;
    public float m_RotationLerpRate = 3.0f;

    private Vector3 m_FollowVelocity;

    void FixedUpdate()
    {
        var position = m_FollowObject.position - m_FollowObject.forward * m_DistanceToObject + Vector3.up * m_Height;
        transform.position = Vector3.SmoothDamp(transform.position, position, ref m_FollowVelocity, m_SmoothMovement);

        transform.forward = Vector3.Lerp(transform.forward, m_FollowObject.forward, m_RotationLerpRate * Time.deltaTime);  
    }
}
