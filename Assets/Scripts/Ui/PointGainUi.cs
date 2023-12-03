using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointGainUi : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI pointText;
    [SerializeField] ObjectPooler pooler;
    [SerializeField] RectTransform targetPoint;
    [SerializeField] float spawnRadius;

    [SerializeField] float scaleTime = 0.1f ,moveTime  = 0.25f;
    [SerializeField] Ease ease = Ease.InSine;
    [SerializeField] Color gainColour, lossColour;



    public int displayPoints;
    
	// Start is called before the first frame update

	private void Start()
	{
		pointText.text = displayPoints.ToString();
	}



	public void OnChangePoints(int points)
    {
        GameObject go = pooler.Spawn();
        DOTween.Kill(go, true);
        RectTransform t = go.GetComponent<RectTransform>();
        t.localScale = Vector3.zero;
        t.anchoredPosition = FindSpawnPoint(!(points < 0));
        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = (points < 0 ? "-" : "+") + Mathf.Abs(points).ToString();
        tmp.color = points < 0 ? lossColour : gainColour;
        PointGainObject point = go.GetComponent<PointGainObject>();
        
		Sequence s = DOTween.Sequence(go.GetComponent<PooledObject>());
        s.Append(t.DOScale(1,scaleTime));
        s.Append(t.DOAnchorPos(targetPoint.anchoredPosition, moveTime).SetEase(ease));
        s.AppendCallback(() => {
            ChangePoints(points);
            go.GetComponent<PooledObject>().Despawn();

        });
    }
    public void ChangePoints(int points)
    {
        displayPoints += points;
        pointText.text = displayPoints.ToString();
    }

	

    Vector2 FindSpawnPoint(bool positive)
    {
		Vector2 rand = Random.insideUnitCircle;
        rand.x = Mathf.Abs(rand.x);
        rand.y = Mathf.Abs(rand.y);
        if(!positive)
            rand.y *= -1;
        return targetPoint.anchoredPosition + rand * spawnRadius;
	}
}
