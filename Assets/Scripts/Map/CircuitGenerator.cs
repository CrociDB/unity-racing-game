using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

public class CircuitGenerator : MonoBehaviour
{
    private MeshFilter m_CircuitMesh;

    private void Awake() 
    {
        m_CircuitMesh = GetComponent<MeshFilter>();    
    }
    
#if UNITY_EDITOR
    public void Generate()
    {
        if (m_CircuitMesh == null)
            m_CircuitMesh = GetComponent<MeshFilter>();

        var mesh = DrawMesh();
        m_CircuitMesh.sharedMesh = mesh;
    }

    private Mesh DrawMesh()
     {
        var points = transform.Cast<Transform>().ToList();

        var width = 8.0f;

        Mesh mesh = m_CircuitMesh.sharedMesh;
        if (mesh == null) mesh = new Mesh();
        mesh.Clear();

        // Generate mesh
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();

        Transform lastPoint = points[points.Count - 1];
        Vector3 lastLeft = Vector3.Cross((points[0].position - lastPoint.position).normalized, Vector3.up);;
        int count = 0;
        foreach (var pos in points)
        {
            var left = Vector3.Cross((pos.position - lastPoint.position).normalized, Vector3.up);

            vertices.Add(lastPoint.position + lastLeft * width);
            vertices.Add(lastPoint.position - lastLeft * width);
            vertices.Add(pos.position + left * width);
            vertices.Add(pos.position - left * width);

            normals.Add(-Vector3.up);
            normals.Add(-Vector3.up);
            normals.Add(-Vector3.up);
            normals.Add(-Vector3.up);

            uv.Add(new Vector3(0.0f, 0.0f));
            uv.Add(new Vector3(1.0f, 0.0f));
            uv.Add(new Vector3(0.0f, 1.0f));
            uv.Add(new Vector3(1.0f, 1.0f));

            triangles.AddRange(new int[]{ 
                count,      count + 2,  count + 1,
                count + 2,  count + 3,  count + 1  });

            count += 4;
            lastPoint = pos;
            lastLeft = left;
        }

        Debug.Log(vertices.Count);
        Debug.Log(triangles.Count);

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
     }


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
