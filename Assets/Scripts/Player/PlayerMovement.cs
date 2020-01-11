using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody m_Body;
    public float m_MaxSpeed = 50.0f;
    public float m_Thrust = 1.0f;

    private void Start() 
    {
        m_Body = GetComponent<Rigidbody>();    
    }

    public void Update()
    {
        m_Body.AddForce(transform.forward * m_Thrust, ForceMode.Acceleration);

        var velocity = m_Body.velocity;
        var speedNormalized = Mathf.Clamp(velocity.sqrMagnitude / (m_MaxSpeed * m_MaxSpeed), 0.3f, 1.0f);

        var turning = Input.GetAxis("Horizontal") * speedNormalized;

        transform.Rotate(transform.up * turning);
        m_Body.AddForce(-m_Body.velocity * Mathf.Abs(turning), ForceMode.Acceleration);

        if (m_Body.velocity.sqrMagnitude >= m_MaxSpeed * m_MaxSpeed)
        {
            m_Body.AddForce(-m_Body.velocity * .4f, ForceMode.Acceleration);
        }
    }
}
