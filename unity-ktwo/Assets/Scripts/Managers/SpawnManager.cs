using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Should exist primarily server-side
// to spawn zombies and players.
public enum SpawnZone
{
    North, South, East, West
}

public class SpawnManager : MonoBehaviour
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
        playerSpawns = new List<GameObject>(); 
        zombieSpawns = new List<GameObject>();
        instance = this;
    }
    
    public void SpawnZombieAtPoint(SpawnZone sa)
    {
        var destination = GameObject.Find(string.Format("ZombieSpawnZone{0}", sa.ToString()));
        var spawn = destination
            .GetComponentsInChildren<Transform>()
            [Random.Range(0, destination.transform.childCount)];
        SpawnEnemy(zombie, spawn.position, spawn.rotation);
    }

    public void SpawnZombieAtRandomPoint()
    {
        var destination = zombieSpawns[Random.Range(0, zombieSpawns.Count)].transform;
        SpawnEnemy(zombie, destination.position, destination.rotation);
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

        SpawnEnemy(zombie, destination.transform.position, destination.transform.rotation);
    }

    void SpawnEnemy(GameObject enemy, Vector3 position, Quaternion rotation)
    {
        var go = Instantiate(enemy, position, rotation);
        NetworkServer.Spawn(go);
        EnemyManager.instance.zombies.Add(go);
    }

    public void SpawnPlayers(Dictionary<NetworkConnection, PlayerConnectionObject> connections)
    {
        Debug.Log(connections.Count);
        Debug.Log(KtwoServer.instance.playerSpot);

        foreach (var kvp in connections)
        {
            var targetString = kvp.Value.playerConnectionSpot != 0 ? 
                string.Format("PlayerSpawnPoint ({0})", kvp.Value.playerConnectionSpot):
                "PlayerSpawnPoint"; 

            Debug.Log(targetString);
            var spawnDestination = GameObject.Find(targetString);

            Debug.Log(CharacterManager.instance.characters);

            GameObject go = Instantiate(
                CharacterManager.instance.characters[kvp.Value.chosenCharacter],
                spawnDestination.transform.position,
                spawnDestination.transform.rotation
            );

            NetworkServer.Spawn(go);
            go.GetComponent<NetworkIdentity>().AssignClientAuthority(kvp.Key);
        }
    }
}
