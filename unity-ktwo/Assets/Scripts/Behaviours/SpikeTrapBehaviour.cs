using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float attackFrequency;

    private bool spikesActive = false;
    private DPSModifier dpsMod;
    private NetworkStructure network;
    
    private Transform spikes;
    private Animator spikeAnimator;

    void Awake()
    {   
        spikes = transform.Find("Spikes");
        spikeAnimator = spikes.GetComponent<Animator>();
        dpsMod = gameObject.GetComponent<DPSModifier>();
        StartCoroutine(CycleSpikes());
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        if (!spikesActive) return;
        other.gameObject.GetComponent<DamagableEnemy>().ServerSideHit(dpsMod.damageAmount, Vector3.zero);
    }

    private IEnumerator CycleSpikes()
    {
        while (true)
        {
            spikeAnimator.SetBool("spikesActive", spikesActive);
            yield return new WaitForSeconds(attackFrequency);
            spikesActive = !spikesActive;
        }
    }
}
