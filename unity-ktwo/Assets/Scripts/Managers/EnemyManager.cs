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

    public void KillAllZombies()
    {
        // Yes... this is really idiotic... unfortunately
        // I can't iterate through the hashset and call
        // die because Die removes the zombies from the 
        // hashset thus modifying the hashset causing
        // an exception. 

        // The solution to this would be to reconsider
        // our needs if zombies really needs to be 
        // a hashset instead of something like a 
        // stack / queue / list.

        // I think its passable for now but will need
        // to be fixed in the future. 
        // - justin
        var zombiesToRemove = new List<GameObject>();

        foreach(var zombie in zombies)
        {
            zombiesToRemove.Add(zombie);
        }
        
        foreach(var zombie in zombiesToRemove)
        {
            zombie.GetComponent<DamagableEnemy>().Die(false);
        }
    }
}
