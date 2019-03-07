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
    [ClientRpc]
    public void RpcLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    bool isPartyLeader = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return; // This belongs to a different player.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
