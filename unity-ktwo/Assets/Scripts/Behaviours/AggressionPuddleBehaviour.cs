using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressionPuddleBehaviour : BasePuddleBehaviour
{
    public int duration;

    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        StartCoroutine(
            other.GetComponent<EnemyController>()
                .TurnAgainstOwn(duration)
        );
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
        numberOfUses -= 1;
    }
}
