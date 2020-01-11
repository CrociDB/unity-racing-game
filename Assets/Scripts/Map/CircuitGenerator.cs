using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(LineRenderer))]
public class CircuitGenerator : MonoBehaviour
{
    private LineRenderer m_CircuitLine;

    private void Awake() 
    {
        m_CircuitLine = GetComponent<LineRenderer>();    
    }
    
    public void Generate()
    {
        var positions = new List<Vector3>();

        foreach (Transform pos in transform)
        {
            positions.Add(pos.position);
            Debug.Log("Got " + positions);
        }

        positions.Add(transform.GetChild(0).position);

        if (m_CircuitLine == null)
            m_CircuitLine = GetComponent<LineRenderer>();  

        m_CircuitLine.positionCount = positions.Count;
        m_CircuitLine.SetPositions(positions.ToArray());
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 last = Vector3.zero;
        int c = 0;
        foreach (Transform pos in transform)
        {
            if (c++ != 0)
            {
                Gizmos.DrawLine(last, pos.position);
            }

            last = pos.position;
        }

        Gizmos.DrawLine(last, transform.GetChild(0).position);

    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(CircuitGenerator))]
public class CircuitGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Circuit"))
        {
            (target as CircuitGenerator).Generate();
        }
    }
}
#endif
