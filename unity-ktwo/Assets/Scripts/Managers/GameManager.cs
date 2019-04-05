using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;

    public GameObject map;
    public float currentWaveTime = 0;
    public bool encounterStarted = false;

    bool waveBegun = false;

    const float WAVE_START_DELAY = 15; // in seconds

    void Awake()
    {
        instance = this;
    }

    public void StartEncounter()
    {
        Debug.Log("starting zombie wave counter");
        StartCoroutine("BeginWave");
        NetworkServer.Spawn(Instantiate(map, Vector3.zero, Quaternion.identity));
        SpawnManager.instance.SpawnPlayers(KtwoServer.instance.connections);
    }

    IEnumerator BeginWave()
    {
        yield return new WaitForSeconds(WAVE_START_DELAY);
        SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.South);
        SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.North);
        SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.East);
        waveBegun = true;
        Debug.Log("wave has begun");
    }
    
    void Update()
    {
        if (waveBegun)
        {
            currentWaveTime += Time.deltaTime;
        }
    }
}
