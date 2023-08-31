using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapGenerator : MonoBehaviour,IDataPersistance<PositionData>
{
	[SerializeField] Mesh mesh;
	[SerializeField] Material material;
	[SerializeField] Material death;
	[SerializeField] float scale = .1f;
	[SerializeField] float scaleDeath = 1f;
	List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();
	List<List<Matrix4x4>> batchesDead = new List<List<Matrix4x4>>();

	private void Update()
	{
		//Instance all spheres
		foreach (List<Matrix4x4> bat in batches)
		{
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Graphics.DrawMeshInstanced(mesh, i, material, bat);
			}
		}
		foreach (List<Matrix4x4> bat in batchesDead)
		{
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Graphics.DrawMeshInstanced(mesh, i, death, bat);
			}
		}
	}

	void IDataPersistance<PositionData>.LoadData(PositionData data)
	{
		List<Matrix4x4> points = new List<Matrix4x4>();
		foreach(Vector3 pos in data.Positions)
		{
			Debug.Log(pos);
			points.Add(Matrix4x4.TRS(pos + Vector3.up, Quaternion.LookRotation(-Vector3.up,Vector3.forward), Vector3.one * scale));
			if(points.Count  == 1000)
			{
				batches.Add(points);
				points = new List<Matrix4x4>();
			}
		}
		batches.Add(points);
		points = new List<Matrix4x4>();
		foreach (Vector3 pos in data.Deaths)
		{
			Debug.Log(pos);
			points.Add(Matrix4x4.TRS(pos + Vector3.up, Quaternion.LookRotation(-Vector3.up, Vector3.forward), Vector3.one * scaleDeath));
			if (points.Count == 1000)
			{
				batchesDead.Add(points);
				points = new List<Matrix4x4>();
			}
		}
		batchesDead.Add(points);

		
	}

	void IDataPersistance<PositionData>.SaveData(ref PositionData data)
	{
		
	}
}
