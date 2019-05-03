using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // local player
    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public List<GameObject> players;

    public static PlayerManager instance;

    void Awake ()
    {
        instance = this;
    }

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
