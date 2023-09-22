using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SecondOrderDynamics
{
    float xp;
    float y, yd;
    float k1, k2, k3;
    

    public SecondOrderDynamics(float f , float z , float r, float x0)
	{
        UpdateKVals(f,z,r);

        

        xp = x0;
        y = x0;
        yd = 0;
	}

    public void UpdateKVals(float f, float z, float r)
	{
        k1 = z / (Mathf.PI * f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);

        //TCrit = 0.8f * (Mathf.Sqrt(4 * k2 + k1 * k1) - k1);
    }



    public float Update(float T , float x)
	{
        if (Time.timeScale == 0)
            return xp;
        float xd = (x - xp) /T;
        xp = x;

        

        float k2Stable = Mathf.Max(k2, T * T / 2 + T * k1 / 2, T*k1);
        y = y + T * yd;
        yd = yd + T * (x + k3 * xd - y - k1 * yd) / k2Stable;
        
        return y;
	}
}
