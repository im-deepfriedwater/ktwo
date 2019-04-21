using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasePuddleBehaviour : BasePuddleBehaviour
{
    NetworkRoot network;
    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject) || !network.isServer) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        Vector3 direction = Quaternion.Euler(0, Random.Range(0, 360), 0) * gameObject.transform.forward;
        other.gameObject.GetComponent<DamagableEnemy>().Hit(0f, direction, false);

        StartCoroutine(
            RemoveFromHashSet(other.gameObject)
        );
        numberOfUses -= 1;
    }
}
