using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake ()
    {
        // No longer valid when we have multiple players.
        // player = GameObject.Find("Player");
        // instance = this;
    }

    #endregion

    [HideInInspector]
    public GameObject player;

    List<GameObject> players;
    

    public GameObject TargetRandomPlayer()
    {
        return players[Random.Range(0, players.Count)];
    }

    public GameObject GetClosestPlayer(Vector3 otherPosition)
    {
        var distance = float.PositiveInfinity;
        GameObject closestPlayer = null;

        foreach (var player in players)
        {
            if (Mathf.Abs(Vector3.Distance(player.transform.position, otherPosition)) < distance)
            {
                closestPlayer = player;
            }
        }

        if (closestPlayer == null)
        {
            throw new System.Exception("No players found!!");
        }
        
        return closestPlayer;
    }

}
