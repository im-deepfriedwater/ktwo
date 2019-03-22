using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager instance;
    public GameObject zombieGroup;
    const float WAVE_DELAY = 15; // in seconds

    void Awake()
    {
        instance = this;
    }

    public void SpawnZombies()
    {   // TODO:
        // Filler code for now. Will be substituted with alejandro's solution later.
        GameObject go = Instantiate(zombieGroup, new Vector3(0.054f, 0.87f, -12f), Quaternion.identity);

        foreach (Transform child in go.transform)
        {
            NetworkServer.Spawn(child.gameObject);
            child.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        }
    }
}
