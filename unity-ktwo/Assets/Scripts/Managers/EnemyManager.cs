using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public HashSet<GameObject> zombies;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        zombies = new HashSet<GameObject>();
    }

    public GameObject GetRandomZombie()
    {
        var target = Random.Range(0, zombies.Count);
        var enumerator = zombies.GetEnumerator();
        for (int i = 0; i < target; i++)
        {
            enumerator.MoveNext();
        }
        return enumerator.Current;
    }

    public void SetGlobalTarget(GameObject target)
    {
        foreach (var zombie in zombies)
        {
            var controller = zombie.GetComponent<EnemyController>();
            if (!controller.turned) controller.target = target;
        }
    }

    public void ResetGlobalTarget()
    {
        foreach (var zombie in zombies)
        {
            var controller = zombie.GetComponent<EnemyController>();
            if (!controller.turned) controller.FindNewTarget();
        }
    }
}
