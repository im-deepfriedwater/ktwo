using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float attackFrequency;
    NetworkRoot network;

    private bool spikesActive = false;
    private DPSModifier dpsMod;
    
    private Transform spikes;
    private Animator spikeAnimator;

    void Start()
    {   
        network = transform.parent.gameObject.GetComponent<NetworkRoot>();
        spikes = transform.Find("Spikes");
        spikeAnimator = spikes.GetComponent<Animator>();
        dpsMod = gameObject.GetComponent<DPSModifier>();
        StartCoroutine(CycleSpikes());
    }

    void OnTriggerStay(Collider other)
    {
        if (!network.isServer || other.gameObject.tag != "Zombie" || !spikesActive) return;
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
