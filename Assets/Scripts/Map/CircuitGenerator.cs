using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

public class CircuitGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [Range(1, 200)]
    public int m_SplineResolution = 1;
    public float m_Width = 12f;
    public float m_Height = 20f;
    private MeshFilter m_CircuitMesh;
    
    public void Generate()
    {
        if (m_CircuitMesh == null)
            m_CircuitMesh = GetComponent<MeshFilter>();

        var mesh = DrawMesh();
        m_CircuitMesh.sharedMesh = mesh;
    }

    private Mesh DrawMesh()
     {
        var points = transform.Cast<Transform>().Select(t => t.position).ToList();

        Mesh mesh = m_CircuitMesh.sharedMesh;
        if (mesh == null) mesh = new Mesh();
        mesh.Clear();

        // Generate mesh
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uv = new List<Vector2>();
        var triangles = new List<int>();

        var lastPoint = points[points.Count - 1];
        var currentPoint = points[0];
        var nextPoint = points[1 % points.Count];
        var nextNextPoint = points[2 % points.Count];

        Vector3 lastPos = GetCatmullRomPosition(1.0f / (float)m_SplineResolution * (float)(m_SplineResolution - 2),
                    points[points.Count - 2],
                    lastPoint,
                    currentPoint,
                    nextPoint);
        
        Vector3 lastLeft = Vector3.Cross((points[0] - lastPoint).normalized, Vector3.up);
        int count = 0;

        for (int j = 0; j < points.Count; j++)
        {
            currentPoint = points[j];
            nextPoint = points[(j + 1) % points.Count];
            nextNextPoint = points[(j + 2) % points.Count];

            for (int i = 0; i < m_SplineResolution; i++)
            {
                var pos = GetCatmullRomPosition(1.0f / (float)m_SplineResolution * (float)i,
                    lastPoint,
                    currentPoint,
                    nextPoint,
                    nextNextPoint);

                var forward = (pos - lastPos).normalized;
                var left = Vector3.Cross(forward, Vector3.up);
                var top = Vector3.Cross(left, forward);

                // the lane
                vertices.Add(lastPos + lastLeft * m_Width);
                vertices.Add(lastPos - lastLeft * m_Width);
                vertices.Add(pos + left * m_Width);
                vertices.Add(pos - left * m_Width);

                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);
                normals.Add(-Vector3.up);

                uv.Add(new Vector3(0.0f, 0.0f));
                uv.Add(new Vector3(0.5f, 0.0f));
                uv.Add(new Vector3(0.0f, 0.5f));
                uv.Add(new Vector3(0.5f, 0.5f));

                triangles.AddRange(new int[]{ 
                        count,      count + 2,  count + 1,
                        count + 2,  count + 3,  count + 1  });

                count += 4;

                // right side
                vertices.Add(lastPos - lastLeft * m_Width);
                vertices.Add(lastPos - lastLeft * m_Width - top * m_Height);
                vertices.Add(pos - left * m_Width);
                vertices.Add(pos - left * m_Width - top * m_Height);

                normals.Add(lastLeft);
                normals.Add(lastLeft);
                normals.Add(left);
                normals.Add(left);

                uv.Add(new Vector3(0.5f, 0.5f));
                uv.Add(new Vector3(0.5f, 0.0f));
                uv.Add(new Vector3(1.0f, 0.5f));
                uv.Add(new Vector3(1.0f, 0.0f));

                triangles.AddRange(new int[]{ 
                        count,      count + 2,  count + 1,
                        count + 2,  count + 3,  count + 1  });

                count += 4;

                // left side
                vertices.Add(lastPos + lastLeft * m_Width);
                vertices.Add(lastPos + lastLeft * m_Width - top * m_Height);
                vertices.Add(pos + left * m_Width);
                vertices.Add(pos + left * m_Width - top * m_Height);

                normals.Add(-lastLeft);
                normals.Add(-lastLeft);
                normals.Add(-left);
                normals.Add(-left);

                uv.Add(new Vector3(0.5f, 0.5f));
                uv.Add(new Vector3(0.5f, 0.0f));
                uv.Add(new Vector3(1.0f, 0.5f));
                uv.Add(new Vector3(1.0f, 0.0f));

                triangles.AddRange(new int[]{ 
                    count,      count + 1,  count + 2,
                    count + 1,  count + 3,  count + 2  });

                count += 4;
                
                lastPos = pos;
                lastLeft = left;
            }

            lastPoint = currentPoint;
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

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        var pointList = transform.Cast<Transform>().Select(t => t.position).ToList();
        
        var lastPoint = pointList.Last();
        var currentPoint = pointList[0];
        var nextPoint = pointList[1 % pointList.Count];
        var nextNextPoint = pointList[2 % pointList.Count];

        Vector3 lastPos = GetCatmullRomPosition(1.0f / (float)m_SplineResolution * (float)(m_SplineResolution - 1), pointList[pointList.Count - 2], lastPoint, currentPoint, nextPoint);

        for (int j = 0; j < pointList.Count; j++)
        {
            currentPoint = pointList[j];
            nextPoint = pointList[(j + 1) % pointList.Count];
            nextNextPoint = pointList[(j + 2) % pointList.Count];

            for (int i = 0; i < m_SplineResolution; i++)
            {
                var pos = GetCatmullRomPosition(1.0f / (float)m_SplineResolution * (float)i, lastPoint, currentPoint, nextPoint, nextNextPoint);
                Gizmos.DrawLine(lastPos, pos);
                Gizmos.DrawSphere(pos, 0.7f);
                lastPos = pos;
            }

            lastPoint = currentPoint;
    
            Gizmos.DrawSphere(currentPoint, 2.0f);
        }
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
