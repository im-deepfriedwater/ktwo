using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAbility : AbstractAbility
{
    public string abilityName;
    public float heightOffset;
    public float distanceFactor; // Distance to spawn from player.
    public GameObject buildPrefab;

    // Update is called once per frame
    protected virtual void Update()
    {
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
            playerTransform.position.x,
            playerTransform.position.y + heightOffset,
            playerTransform.position.z
        );
        newPosition += playerTransform.forward * distanceFactor;
        player.CmdBuildObject(buildPrefab.name, newPosition, playerTransform.rotation);
    }

} 
