using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatMapGenerator : MonoBehaviour,IDataPersistance<PositionData>
{
	[SerializeField] Mesh mesh;
	[SerializeField] Material material;
	[SerializeField] Material death;
	[SerializeField] float scale = .1f;
	[SerializeField] float scaleDeath = 1f;
	[SerializeField] Slider scaleSlider;
	[SerializeField] Slider alphaSlider;


	List<List<Matrix4x4>> batches = new List<List<Matrix4x4>>();

	List<List<Matrix4x4>> batchesDead = new List<List<Matrix4x4>>();
	[SerializeField] float flatHeight;
	public bool flatMap;
	public bool useVel;
	PositionData data;

	private void Awake()
	{

	}

	private void Update()
	{
		//Instance all spheres
		int j = 0;
		foreach (List<Matrix4x4> bat in batches)
		{
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				Graphics.DrawMeshInstanced(mesh, i, material, bat);
			}
			j++;
		}
		foreach (List<Matrix4x4> bat in batchesDead)
		{
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				
				Graphics.DrawMeshInstanced(mesh, i, death, bat);
				
			}
		}
	}
	public void UpdateScale()
	{
		scale = scaleSlider.value;
	}
	public void UpdateAlpha()
	{
		Color col = new Color(material.color.r,material.color.g,material.color.b,alphaSlider.value);
		material.SetColor("_Color", col);
	}

	public void FlatenMap(bool a)
	{
		flatMap = a;
	}

	public void UseVelocity(bool a)
	{
		useVel = a;
	}

	void IDataPersistance<PositionData>.LoadData(PositionData data)
	{
		this.data = data;
		LoadMap();
	}

	public void LoadMap()
	{
		batches.Clear();
		
		batchesDead.Clear();
		List<Matrix4x4> points = new List<Matrix4x4>();
		int i = 0;
		foreach (Vector3 pos in data.Positions)
		{
			//Debug.Log(pos);
			Vector3 thisPos = pos;
			if (flatMap)
				thisPos.y = flatHeight;
			points.Add(Matrix4x4.TRS(thisPos + Vector3.up, Quaternion.LookRotation(-Vector3.up, Vector3.forward), Vector3.one * (useVel? scale * data.velocity[i]: scale)));

			if (points.Count == 1023)
			{
				batches.Add(points);
				points = new List<Matrix4x4>();
			}
			i++;
		}

		batches.Add(points);
		points = new List<Matrix4x4>();


		

		foreach (Vector3 pos in data.Deaths)
		{
			Vector3 thisPos = pos;
			if (flatMap)
				thisPos.y = flatHeight;
			points.Add(Matrix4x4.TRS(thisPos + Vector3.up, Quaternion.LookRotation(-Vector3.up, Vector3.forward), Vector3.one * scaleDeath));
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
