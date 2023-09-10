using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    Mesh m1;
    [SerializeField] Mesh m2,m3;
    
    SkinnedMeshRenderer mRenderer;
    [Range(0.0f, 1.0f)]
    [SerializeField] float interp = 1;

    [Range(0.0f, 1.0f)]
    [SerializeField] float a = 1;

    [Range(0.0f, 1.0f)]
    [SerializeField] float b = 1;
    Vector3[]m1v, m2v, m3v;
    Color[] cols,newcols;
    // Start is called before the first frame update
    void Awake()
    {
        
        
        mRenderer = GetComponent<SkinnedMeshRenderer>();
        
        m1 = new Mesh();
        m1.vertices = m2.vertices;
        m1.boneWeights = m2.boneWeights;
        m1.bindposes = m2.bindposes;
        m1.uv = m2.uv;
        m1.normals = m2.normals;
        m1.triangles = m2.triangles;

        mRenderer.sharedMesh = m1;
        m1v = m2.vertices;
        m2v = m2.vertices;
        m3v = m3.vertices;
        cols = m2.colors;
        newcols = m2.colors;

        m1.colors = cols;
        m1.uv = m2.uv;
    }

    // Update is called once per frame
    void Update()
    {
        //interp = (Mathf.Sin(Time.time) + 1)/2;

        for(int i = 0; i < m1v.Length; i++)
		{
            float val = GetValue(cols[i].r);
            newcols[i] = Color.white * val;
            m1v[i] = Vector3.Lerp(m2v[i], m3v[i],val);
		}
        m1.vertices = m1v;
        m1.colors=newcols;
    }

    float GetValue(float t)
	{
        float val = ((t*t) + 1) * interp;
        if (val < a)
            val = 0;
        if(val > b)
            val = 1;


        val = Mathf.Clamp01(val);
        return val;
	}
}
