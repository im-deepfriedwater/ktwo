using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GreasePuddleBehaviour : BasePuddleBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        Vector3 direction = Quaternion.Euler(0, Random.Range(0, 360), 0) * gameObject.transform.forward;
        other.gameObject.GetComponent<DamagableEnemy>().Hit(0f, direction, true);

        StartCoroutine(
            RemoveFromHashSet(other.gameObject)
        );
        numberOfUses -= 1;
    }
}
