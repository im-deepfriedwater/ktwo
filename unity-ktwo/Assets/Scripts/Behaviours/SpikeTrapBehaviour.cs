using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float attackFrequency;

    private bool spikesActive = true;
    private DPSModifier dpsMod;
    
    void Awake()
    {   
        dpsMod = gameObject.GetComponent<DPSModifier>();
        StartCoroutine(CycleSpikes());
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        if (!spikesActive) return;
        other.gameObject.GetComponent<DamagableEnemy>().Hit(dpsMod.damageAmount, Vector3.zero);
    }

    private IEnumerator CycleSpikes()
    {
        while (true)
        {
            spikesActive = !spikesActive;
            yield return new WaitForSeconds(attackFrequency);
        }
    }
}
