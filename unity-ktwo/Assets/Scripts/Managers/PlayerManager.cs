using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    void Awake ()
    {
        player = GameObject.Find("Player");
        instance = this;
    }

    #endregion

    [HideInInspector]
    public GameObject player;

}
