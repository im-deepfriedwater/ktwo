using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpfulPuddle : MonoBehaviour
{
    public int numberOfUses;
    public float speedBoostPercent;
    public float buffDuration;
    public float speedDebuffPercent;
    public float debuffDuration;

    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();
    
    void OnTriggerStay(Collider other)
    {
        // if (numberOfUses == 0)
        // {
        //     Destroy(gameObject);
        // }
        // numberOfUses -= 1;

        if (affectedEntities.Contains(other.gameObject)) return;

        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            // +15% run speed for 8 seconds
            StartCoroutine(
                other.GetComponent<PlayerBehaviour>().
                TimedAffectSpeed(speedBoostPercent, buffDuration, true, affectedEntities)
            );
        }

        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);
            // invincibility for 2 seconds
            // StartCoroutine(
            //     if a structure has one,
            //     Call each structure's MakeInvincible(float time)
            // );
            
            // +25% DPS for 8 seconds
            // StartCoroutine(
            //     if a structure has one,
            //     Call each structure's AffectDPS(float percent, float time)
            // );
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            // -15% run speed for 5 seconds
            StartCoroutine(
                other.GetComponent<EnemyController>().
                TimedAffectSpeed(speedDebuffPercent, debuffDuration, false, affectedEntities)
            );
        }
    }
}
