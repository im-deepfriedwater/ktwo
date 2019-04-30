﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildPlayerAbility : AbstractAbility
{
    public string abilityName;
    public float heightOffset;
    public float distanceFactor; // Distance to spawn from player.
    public GameObject buildPrefab;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player.isDead) return;

        if (inputState.GetButtonValue(inputButtons[0]) && cooldownOver)
        {
            cooldownOver = false;
            StartCoroutine("WaitForCooldown");
            CmdBuildParentedObject(abilityName);
        }
        UpdateAbilityUI();
    }

    [Command]
    void CmdBuildParentedObject(string name)
    {
        var newPosition = new Vector3(
            transform.position.x,
            transform.position.y + heightOffset,
            transform.position.z
        );

        newPosition += transform.forward * distanceFactor;

        var go = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)), newPosition, transform.rotation);
        NetworkServer.Spawn(go);
        go.transform.parent = transform;
        RpcSetParent(gameObject.GetComponent<NetworkIdentity>(), go.GetComponent<NetworkIdentity>());
    }

    [ClientRpc]
    void RpcSetParent(NetworkIdentity parent, NetworkIdentity child)
    {
        child.gameObject.transform.parent = parent.gameObject.transform;
    }
}