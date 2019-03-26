using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammablePuddleBehaviour : MonoBehaviour
{

    public int numberOfUses;
    public int DPS;
    public int duration;

    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();

    void Update()
    {
        if (numberOfUses == 0 && affectedEntities.Count == 0) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (numberOfUses == 0) return;
        if (affectedEntities.Contains(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);

        StartCoroutine(
            other.GetComponent<DamagableEnemy>()
                .DamageOverTime(DPS, duration)
        );
        StartCoroutine(
            other.GetComponent<DamagableEnemy>()
                .SetOnFire(duration)
        );
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
        numberOfUses -= 1;
    }

    IEnumerator RemoveFromHashSet(GameObject entity, float time)
    {
        yield return new WaitForSeconds(time);
        affectedEntities.Remove(entity);
    }
}
