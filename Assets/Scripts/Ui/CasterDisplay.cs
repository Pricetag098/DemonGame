using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class CasterDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] abilityIconIdentifier;
    [SerializeField] Slider bloodMeter;
    [SerializeField] CanvasGroup onEmptyVisualization;
    
    [SerializeField] PlayerAbilityCaster caster;

    [SerializeField] float frequncey;
    [SerializeField] float damping;
    [SerializeField] float theOtherOne;
    SecondOrderDynamics dynamics;
    public bool isEmpty = false;

    // Start is called before the first frame update
    void Awake()
    {
        caster = FindObjectOfType<PlayerAbilityCaster>();
        
    }

	private void Start()
	{
        dynamics = new SecondOrderDynamics(frequncey, damping, theOtherOne, caster.caster.blood / caster.caster.maxBlood);
    }

	// Update is called once per frame
	void Update()
    {
        for(int i = 0; i < abilityIconIdentifier.Length; i++)
		{
            abilityIconIdentifier[i].text = caster.caster.abilities[i].symbolText;
            //abilityIconIdentifier[i].GetComponent<Outline>().enabled = i == caster.activeIndex;
		}
        bloodMeter.value = dynamics.Update(Time.unscaledDeltaTime,caster.caster.blood / caster.caster.maxBlood);

        if(bloodMeter.value <= 0.1f && !isEmpty)
        {
            TweenEmptyVisals();
        }
        else if (bloodMeter.value  > 0.1f)
        {
            isEmpty = false;
            onEmptyVisualization.DOKill();
            onEmptyVisualization.alpha = 0f;
        }
    }

    private void TweenEmptyVisals()
    {
        isEmpty = true;
        onEmptyVisualization.DOFade(1, .8f).SetLoops(-1, LoopType.Yoyo);
    }
}
