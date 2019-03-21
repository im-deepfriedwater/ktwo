﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// A virtual object meant to represent a client that has connected.
// We control state of each client here and have to be careful to
// make sure each client loads the same scenes and is in the same states.
public class PlayerConnectionObject : NetworkBehaviour
{

    public GameObject PlayerUnitPrefab;
    public GameObject PlayerSystems;

    [ClientRpc]
    public void RpcLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    [Command]
    void CmdSpawn()
    {
        Debug.Log(connectionToClient);
        GameObject go = Instantiate(PlayerUnitPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
        NetworkServer.Spawn(go);
        go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
    bool isPartyLeader = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return; // This belongs to a different player.
        }
        Instantiate(PlayerSystems, Vector3.zero, Quaternion.identity);
        CmdSpawn();
    }
}
