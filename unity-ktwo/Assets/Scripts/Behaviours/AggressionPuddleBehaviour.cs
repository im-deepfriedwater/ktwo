using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressionPuddleBehaviour : MonoBehaviour
{
    public int numberOfUses;
    public int duration;

    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();
        
    void Update()
    {
        if (numberOfUses == 0 && affectedEntities.Count == 0) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (numberOfUses == 0) return;
        if (affectedEntities.Contains(other.gameObject)) return;

        if (other.gameObject.tag != "Zombie") return;
        affectedEntities.Add(other.gameObject);
        
        StartCoroutine(
            other.GetComponent<EnemyController>().
            TurnAgainstOwn(duration, affectedEntities)
        );
        numberOfUses -= 1;
    }
}
