using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PositionRecorder : MonoBehaviour,IDataPersistance<PositionData>
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> deaths = new List<Vector3>();
    [SerializeField] float positionRecordFreq = .1f;
    float timer;

	private void Awake()
	{
        GetComponent<Health>().OnDeath += OnDeath;
	}
	void OnDeath()
	{
        deaths.Add(transform.position);
	}
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > positionRecordFreq)
		{
            timer = 0;
            positions.Add(transform.position);
		}
    }

    void IDataPersistance<PositionData>.LoadData(PositionData data)
	{
	}

    void IDataPersistance<PositionData>.SaveData(ref PositionData data)
	{

        data.Positions = data.Positions.Concat(positions.ToArray()).ToArray();
        data.Deaths = data.Deaths.Concat(deaths.ToArray()).ToArray();
	}
}
