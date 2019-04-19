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
    // Chosen Character Mapping
    // 0 = Arhictect
    // 1 = Chemist
    // 2 = Chef
    // 3 = Dog
    // 4 = Tinkerer

    public int chosenCharacter;

    [SyncVar]
    bool isPartyLeader = false; // is true for the first person to connect

    // Given to the player in terms of which player they are.
    // E.G.: first player to connect is 0, 2nd is 1 etc...
    public int connectionNumber;
    
    void Start()
    {
        if (isServer) 
        {
            RpcInitializeCSS(KtwoServer.instance.playerSpot++);
        }

        if (hasAuthority)
        {
            CSSManager.instance.localPlayer = this;
        }
    }

    [ClientRpc]
    public void RpcLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    [ClientRpc]
    public void RpcInitializeCSS(int connectionNumber)
    {
        // if (!hasAuthority) return;
        Debug.Log("this happened");
        this.connectionNumber = connectionNumber;
        CSSManager.instance.connectionNumber = connectionNumber;
        CSSManager.instance.ShowCSSScreen();
    }

    [Command]
    public void CmdUpdateChosenCharacter(int character)
    {
        chosenCharacter = character;
    }

    [ClientRpc]
    public void RpcInitializeForEncounter()
    {
        if (!hasAuthority) 
        {
            Debug.Log("skipping tho");
            return;
        }
        CSSManager.instance.HideCSSScreen();
    }
}
