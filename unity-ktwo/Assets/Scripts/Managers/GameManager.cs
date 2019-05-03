using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;

    public GameObject map;
    public GameObject networkUIManager;
    public float currentWaveTime = 0;
    public bool encounterStarted = false;

    public const float WAVE_START_DELAY = 15; // in seconds

    void Awake()
    {
        instance = this;
    }

    public void StartEncounter()
    {
        StartCoroutine(BeginWave());
        TellClientsToInitializeForEncounter();
        NetworkServer.Spawn(Instantiate(networkUIManager, Vector3.zero, Quaternion.identity));
        NetworkServer.Spawn(Instantiate(map, Vector3.zero, Quaternion.identity));
        SpawnManager.instance.SpawnPlayers(KtwoServer.instance.connections);
    }

    void TellClientsToInitializeForEncounter()
    {
        foreach (var kvp in KtwoServer.instance.connections)
        {
            kvp.Value.RpcInitializeForEncounter();
        }
    }

    IEnumerator BeginWave()
    {
        yield return new WaitForSeconds(WAVE_START_DELAY);
        WaveManager.instance.BeginWave();
    }

}
