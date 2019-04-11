using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildAbility : AbstractAbility
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
            SpawnInFrontOfPlayer();
        }
        UpdateAbilityUI();
    }

    protected virtual void SpawnInFrontOfPlayer()
    {
        var newPosition = new Vector3(
            transform.position.x,
            transform.position.y + heightOffset,
            transform.position.z
        );
        newPosition += transform.forward * distanceFactor;
        CmdBuildObject(buildPrefab.name, newPosition, transform.rotation);
    }

    [Command]
    public void CmdBuildObject(string name, Vector3 position, Quaternion rotation)    
    {      
        var go = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)), position, rotation);
        NetworkServer.Spawn(go);
    }
} 
