using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public Transform transformA, transformB;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;

    List<Vector3> pointsA = new List<Vector3>(), pointsB = new List<Vector3>();
    List<float> times = new List<float>();

    public float maxAge = .1f;

    //public float minDistance;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pointsA.Add(transform.InverseTransformPoint(transformA.position));
        pointsB.Add(transform.InverseTransformPoint(transformB.position));
        times.Add(Time.time);

        while (Time.time - times[0] > maxAge && times.Count > 2)
        {
            pointsA.RemoveAt(0);
            pointsB.RemoveAt(0);
            times.RemoveAt(0);
        }

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        List<float> aPathDistances = new List<float>();
        List<float> bPathDistances = new List<float>();
        float aPathDist = 0;
        float bPathDist = 0;

        for (int i = 0; i < pointsA.Count; i++)
        {

            float x = ((float)i) / ((float)pointsA.Count);
            verts.Add(pointsA[i]);
            verts.Add(pointsB[i]);

            if (i > 0)
            {
                float pDistA = Vector3.SqrMagnitude(pointsA[i] - pointsA[i - 1]);
                float pDistB = Vector3.SqrMagnitude(pointsB[i] - pointsB[i - 1]);
                aPathDist += pDistA;
                bPathDist += pDistB;
                aPathDistances.Add(pDistA);
                bPathDistances.Add(pDistB);
            }
            else
            {
                aPathDistances.Add(0);
                bPathDistances.Add(0);
            }
            //uvs.Add(new Vector2(x, 0));
            //uvs.Add(new Vector2(x, 1));
        }
        float aDist = 0;
        float bDist = 0;
        for (int i = 0; i < pointsA.Count; i++)
        {
            aDist += aPathDistances[i];
            bDist += bPathDistances[i];
            uvs.Add(new Vector2(aDist / aPathDist, 0));
            uvs.Add(new Vector2(bDist / bPathDist, 1));
        }

        verts.Add(transformA.position);
        verts.Add(transformB.position);
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        int triCount = (pointsA.Count - 1) * 2;
        for (int i = 0; i < triCount; i += 2)
        {
            tris.Add(i + 0);
            tris.Add(i + 1);
            tris.Add(i + 2);

            tris.Add(i + 2);
            tris.Add(i + 1);
            tris.Add(i + 3);

        }
        mesh.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
    }

    //private void OnDrawGizmos()
    //{
    //	Gizmos.color = Color.red;

    //       for(int i = 0; i < pointsA.Count;i++)
    //       {
    //           Gizmos.DrawSphere(pointsA[i],.01f);
    //		Gizmos.DrawSphere(pointsB[i], .01f);
    //	}



    //}
}
