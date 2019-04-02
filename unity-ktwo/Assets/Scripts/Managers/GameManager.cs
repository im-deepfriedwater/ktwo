using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager: NetworkBehaviour
{
    public static GameManager instance;

    public float currentWaveTime = 0;

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
    }

    IEnumerator BeginWave()
    {
        yield return new WaitForSeconds(WAVE_START_DELAY);
        NetworkServer.Spawn(SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.South));
        NetworkServer.Spawn(SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.North));
        NetworkServer.Spawn(SpawnManager.instance.SpawnZombieAtPoint(SpawnZone.East));

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
