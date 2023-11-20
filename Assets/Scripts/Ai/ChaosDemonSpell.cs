using DigitalRuby.ThunderAndLightning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosDemonSpell : MonoBehaviour
{
    float timer;
    float activationTime;
    float damage;
    [SerializeField] float hitRadius = 1;
    [SerializeField] LayerMask targetLayers, wallLayers;
    [SerializeField]LightningBoltPrefabScript strikeLightning;
    [SerializeField] float boltOriginHeight;
    bool struck = false;


    public void Spawn(float activationTime, Vector3 spawnPoint,float damage)
    {
        this.damage = damage;
        this.activationTime = activationTime;

        timer = 0;
        struck = false;
        transform.position = spawnPoint;
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit,100,wallLayers))
        {
            transform.position = hit.point;
        }
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > activationTime && !struck)
        {
            strikeLightning.Trigger(transform.position, new Vector3(transform.position.x, boltOriginHeight, transform.position.z));
            struck = true;

            Collider[] colliders = Physics.OverlapCapsule(transform.position, new Vector3(transform.position.x, boltOriginHeight, transform.position.z),hitRadius,targetLayers);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent(out HitBox hitBox))
                {
                    hitBox.OnHit(damage, HitType.ABILITY);

                }
            }
        }
    }
}
