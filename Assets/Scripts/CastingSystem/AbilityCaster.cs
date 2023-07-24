using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public float blood;
    public float maxBlood;
    public Ability[] abilities;
    public int activeIndex;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Equip(this);
        }
    }
	private void OnEnable()
	{
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].EnableInputs();
        }
    }
	private void OnDisable()
	{
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].DisableInputs();
        }
    }


	// Update is called once per frame
	void Update()
    {
        for(int i = 0; i < abilities.Length; i++)
		{
            abilities[i].Tick(i == activeIndex);
		}
    }

    public void AddBlood(float amount)
	{
        blood = Mathf.Clamp(blood + amount, 0, maxBlood);
	}
}
