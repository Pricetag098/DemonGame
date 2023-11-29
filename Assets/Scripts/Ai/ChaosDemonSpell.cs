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
    [SerializeField] LightningBoltPrefabScript strikeLightning;
    [SerializeField] GameObject circle;
    [SerializeField] SoundPlayer strikeSound;
    [SerializeField] float boltOriginHeight;
    bool struck = false;


    public void Spawn(float activationTime, Vector3 spawnPoint,float damage)
    {
        this.damage = damage;
        this.activationTime = activationTime;
        circle.SetActive(true);
        timer = 0;
        struck = false;
        transform.position = spawnPoint;
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit,10000000,wallLayers))
        {
            transform.position = hit.point;
        }
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        List<Health> healths = new List<Health>();
        timer += Time.deltaTime;
        if(timer > activationTime && !struck)
        {
            
            strikeLightning.Trigger(new Vector3(transform.position.x, boltOriginHeight, transform.position.z), transform.position);
            struck = true;
            strikeSound.Play();
            Collider[] colliders = Physics.OverlapCapsule(transform.position, new Vector3(transform.position.x, boltOriginHeight, transform.position.z),hitRadius,targetLayers);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent(out HitBox hitBox))
                {
                    if (!healths.Contains(hitBox.health))
                    {
                        healths.Add(hitBox.health);
                        hitBox.OnHit(damage, HitType.ABILITY);
                    }
                    

                }
            }
            circle.SetActive(false);
        }
    }
}
