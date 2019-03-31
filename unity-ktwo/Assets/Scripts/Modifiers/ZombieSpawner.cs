using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attaching this script to an object makes 
// that object a potential zombie spawner.
public class ZombieSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnManager.instance.spawns.Add(this.gameObject);
    }
}
