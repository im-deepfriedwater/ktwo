using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class KtwoServer: NetworkManager
{    

    public static KtwoServer instance;
    public int playerSpot = 0;
    // We can use the count of connections to represent player ids.
    public Dictionary<NetworkConnection, PlayerConnectionObject> connections;
    
    public void Awake ()
    {
        connections = new Dictionary<NetworkConnection, PlayerConnectionObject>();
        instance = this;
    }

    public void Start ()
    {
        StartServer();
    }

    // Server callbacks
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("A client connected to the server: " + conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkServer.DestroyPlayersForConnection(conn);
        if (conn.lastError != NetworkError.Ok)
        {
            if (LogFilter.logError)
            { 
                Debug.LogError("ServerDisconnected due to error: " + conn.lastError); 
            }
        }
        Debug.Log("A client disconnected from the server: " + conn);
        connections.Remove(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SetClientReady(conn);
        Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("Client has requested to get his player added to the game");
        var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        var connectionObject = player.GetComponent<PlayerConnectionObject>();
        connections[conn] = connectionObject;
        GameObject.Find("PlayerConnectionTextGroup").GetComponent<TextUpdater>().UpdateText();
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        if (player.gameObject != null)
        {
            NetworkServer.Destroy(player.gameObject);
        }

        connections.Remove(conn);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("Server network error occurred: " + (NetworkError)errorCode);
    }

    public override void OnStartServer()
    {
        Debug.Log("Server has started");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server has stopped");
    }
}
