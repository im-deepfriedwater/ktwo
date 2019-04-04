using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePuddleBehaviour : BasePuddleBehaviour
{
    public int knockbackForce;

    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        // knock zombies back

        // StartCoroutine(
        //     RemoveFromHashSet(other.gameObject, duration)
        // );
        numberOfUses -= 1;
    }
}
