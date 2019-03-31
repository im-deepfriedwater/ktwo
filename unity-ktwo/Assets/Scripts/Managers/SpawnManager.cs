using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnZone
{
    North, South, East, West
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public GameObject zombie;
    public List<GameObject> spawns;
    public SpawnZone StartSpawnZone;
    public bool SpawnOnStartup;
    const float WAVE_DELAY = 10; // in seconds


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (SpawnOnStartup)
        {
            SpawnZombieAtPoint(StartSpawnZone);
        }
    }
    
    // Returns a 
    public GameObject SpawnZombieAtPoint(SpawnZone sa)
    {
        var destination = GameObject.Find(string.Format("ZombieSpawnZone{0}", sa.ToString()));
        var spawn = destination
            .GetComponentsInChildren<Transform>()
            [Random.Range(0, destination.transform.childCount)];
        return Instantiate(zombie, spawn.transform.position, spawn.transform.rotation);
    }

    public GameObject SpawnZombieAtRandomPoint()
    {
        var destination = spawns[Random.Range(0, spawns.Count)].transform;
        return Instantiate(zombie, destination.position, destination.rotation);
    }

    public GameObject SpawnAtClosest(Transform t)
    {
        var shortest = Mathf.Infinity; 
        GameObject destination = null;
        foreach (GameObject go in spawns)
        {
            var distance = Mathf.Abs(Vector3.Distance(t.transform.position, go.transform.position));

            if (distance < shortest)
            {
                destination = go;
                shortest = distance;
            }
        }

        return Instantiate(zombie, destination.transform.position, destination.transform.rotation);
    }
}
