//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MeleeWeapon : Ability
//{
//    [Header("Melee Settings")]
//    public float swingRad;

//    protected override void Cast()
//    {

//        Vector3 dir = Camera.main.transform.forward;
//        RaycastHit[] hits = Physics.SphereCastAll(Camera.main.transform.position, swingRad,dir , bulletRange, hitMask);
//        if(hits.Length > 0)
//        {
//            List<Health> healths = new List<Health>();
//            for (int i = 0; i < hits.Length; i++)
//            {
//                RaycastHit hit = hits[i];
//                HitBox hitBox;
//                bool playFx = true;
//                if (hit.collider.TryGetComponent(out hitBox))
//                {

//                    if (healths.Contains(hitBox.health))
//                    {
//                        playFx = false;
//                    }
//                    else
//                    {
//                        healths.Add(hitBox.health);
//                        hitBox.OnHit(damage * holster.stats.damageMulti);
//                        holster.OnHit(damage * holster.stats.damageMulti * hitBox.multi);
//                    }


//                }
//                HitSettings hitSettings;
//                if (hit.collider.TryGetComponent(out hitSettings))
//                {
//                    if (playFx)
//                        hitSettings.PlayVfx(hit.point, Vector3.Lerp(-dir, hit.normal, .5f));
//                }
//                else
//                {
//                    if (playFx)
//                        VfxSpawner.SpawnVfx(0, hit.point, Vector3.Lerp(-dir, hit.normal, .5f));
//                }
//            }
//        }
//    }


//    private void OnDrawGizmos()
//    {
        
//    }
//}
