using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePuddleBehaviour : MonoBehaviour
{
    [Header("Puddle Settings")]
    public int numberOfUses;
    public float decayRate;

    public HashSet<GameObject> affectedEntities = new HashSet<GameObject>();

    protected void Awake()
    {
        StartCoroutine(PuddleLifetime(decayRate));
    }

    void Update()
    {
        if (numberOfUses <= 0 && affectedEntities.Count == 0) Destroy(gameObject);
    }

    public bool CannotBeUsed(GameObject entity)
    {
        return numberOfUses <= 0 || affectedEntities.Contains(entity);
    }

    IEnumerator PuddleLifetime(float time)
    {
        while (numberOfUses > 0)
        {
            yield return new WaitForSeconds(time);
            numberOfUses -= 1;
        }
    }

    public IEnumerator RemoveFromHashSet(GameObject entity, float time = 0f)
    {
        yield return new WaitForSeconds(time);
        affectedEntities.Remove(entity);
    }
}
