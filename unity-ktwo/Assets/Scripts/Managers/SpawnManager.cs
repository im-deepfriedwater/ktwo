using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Should exist primarily server-side.
// Clients generally should not be
// spawning entites through this.
public enum SpawnZone
{
    North, South, East, West
}

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager instance;
    public GameObject zombie;
    public List<GameObject> playerSpawns;
    public List<GameObject> zombieSpawns;
    public SpawnZone StartSpawnZone;
    public bool SpawnOnStartup;
    const float SPAWN_DELAY = 1.5f;
    const float WAVE_DELAY = 10; // in seconds

    public List<GameObject> characterMapping;


    void Awake()
    {
        instance = this;
    }
    
    public void SpawnZombieAtPoint(SpawnZone sa)
    {
        var destination = GameObject.Find(string.Format("ZombieSpawnZone{0}", sa.ToString()));
        Debug.Log(string.Format("ZombieSpawnZone{0}", sa.ToString()));
        var spawn = destination
            .GetComponentsInChildren<Transform>()
            [Random.Range(0, destination.transform.childCount)];
        NetworkServer.Spawn(Instantiate(zombie, spawn.transform.position, spawn.transform.rotation));
    }

    public void SpawnZombieAtRandomPoint()
    {
        var destination = zombieSpawns[Random.Range(0, zombieSpawns.Count)].transform;
        NetworkServer.Spawn(Instantiate(zombie, destination.position, destination.rotation));
    }

    public void SpawnMultipleZombiesAtRandomPoint(int numberOfZombies)
    {
        StartCoroutine(TimedSpawn(numberOfZombies));
    }

    public IEnumerator TimedSpawn(int numberOfZombies)
    {
        var counter = 0;
        while(counter < numberOfZombies)
        {
            yield return new WaitForSeconds(SPAWN_DELAY);
            SpawnZombieAtRandomPoint();
            counter--;
        }
    }

    public void SpawnAtClosest(Transform t)
    {
        var shortest = Mathf.Infinity; 
        GameObject destination = null;
        foreach (GameObject go in zombieSpawns)
        {
            var distance = Mathf.Abs(Vector3.Distance(t.transform.position, go.transform.position));

            if (distance < shortest)
            {
                destination = go;
                shortest = distance;
            }
        }

        NetworkServer.Spawn(Instantiate(zombie, destination.transform.position, destination.transform.rotation));
    }

    // TODO
    public GameObject[] SpawnPlayers(GameObject[] players)
    {
        throw new System.NotImplementedException();
    }
}
