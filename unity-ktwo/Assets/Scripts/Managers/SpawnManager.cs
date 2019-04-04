using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Should exist primarily server-side
// to spawn zombies and players.
// Clients should not be spawning 
// entites through this.
public enum SpawnZone
{
    North, South, East, West
}

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager instance;

    public List<GameObject> playerSpawns;
    public List<GameObject> zombieSpawns;
    
    public GameObject zombie;
    public SpawnZone StartSpawnZone;

    public bool SpawnOnStartup;

    const float SPAWN_DELAY = 1.5f;
    const float WAVE_DELAY = 10; // in seconds

    void Awake()
    {
        instance = this;
    }
    
    public void SpawnZombieAtPoint(SpawnZone sa)
    {
        var destination = GameObject.Find(string.Format("ZombieSpawnZone{0}", sa.ToString()));
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

    public void SpawnPlayers(Dictionary<NetworkConnection, PlayerConnectionObject> connections)
    {
        foreach (var kvp in connections)
        {
            var spawnDestination = GameObject.Find(string.Format("PlayerSpawnPoint {0}", kvp.Value.playerConnectionSpot));

            GameObject go = Instantiate(
                CharacterManager.instance.characters[kvp.Value.chosenCharacter], 
                new Vector3(0, 0.5f, 0), 
                Quaternion.identity
            );

            NetworkServer.Spawn(go);
            go.GetComponent<NetworkIdentity>().AssignClientAuthority(kvp.Key);
        }
    }
}
