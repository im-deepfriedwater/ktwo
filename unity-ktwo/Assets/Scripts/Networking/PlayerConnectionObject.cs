using System.Collections;
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

    // Chosen Character Mapping
    // 0 = Arhictect
    // 1 = Chemist
    // 2 = Chef
    // 3 = Dog
    // 4 = Tinkerer

    [SyncVar]
    public int chosenCharacter;

    [SyncVar]
    bool isPartyLeader = false; // is true for the first person to connect

    [SyncVar]
    public int playerConnectionSpot; // Ascending order, 0 is first, 1 is second etc

    [ClientRpc]
    public void RpcLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void SpawnPlayer() // Gets called by serverside when encounter starts.
    {
        if (!isLocalPlayer)
        {
            return; // This belongs to a different player.
        }

        Instantiate(PlayerSystems, Vector3.zero, Quaternion.identity);
        GameObject go = Instantiate(PlayerUnitPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
        NetworkServer.Spawn(go);
        go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
}
