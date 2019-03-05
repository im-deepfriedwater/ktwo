using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float damageAmount;

    private bool spikesActive = true;
    
    void Awake()
    {   
        StartCoroutine(CycleSpikes());
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        if (!spikesActive) return;
        other.gameObject.GetComponent<DamagableEnemy>().Hit(damageAmount, Vector3.zero);
    }

    private IEnumerator CycleSpikes()
    {
        while (true){
            spikesActive = !spikesActive;
            Debug.Log("spikesActive: " + spikesActive);
            yield return new WaitForSeconds(3.0f);
        }
    }
}
