using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammablePuddleBehaviour : BasePuddleBehaviour
{
    public int DPS;
    public int duration;

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        StartCoroutine(
            other.GetComponent<DamagableEnemy>()
                .DamageOverTime(DPS, duration)
        );
        other.GetComponent<DamagableEnemy>().RpcSetOnFire(duration);
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
        numberOfUses -= 1;
    }
}
