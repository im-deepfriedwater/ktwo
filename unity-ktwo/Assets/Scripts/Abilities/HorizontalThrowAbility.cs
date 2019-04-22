using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HorizontalThrowAbility : AbstractAbility
{
    public string abilityName;
    public float heightOffset;
    public float throwDistance; // Distance to spawn from player.
    public float projectileVelocity;
    public GameObject projectilePrefab;

    private Transform projectileTransform;

    protected virtual void Update()
    {
        if (player.isDead) return;

        if (inputState.GetButtonValue(inputButtons[0]) && cooldownOver)
        {
            cooldownOver = false;
            StartCoroutine("WaitForCooldown");
            CmdThrowProjectile(projectilePrefab.name);
        }
        UpdateAbilityUI();
    }

    IEnumerator ThrowProjectileFromPlayer(string toSpawn)
    {
        var projectile = (GameObject)Instantiate(
            Resources.Load(toSpawn, typeof(GameObject)),
            transform.position + transform.forward + new Vector3(0, heightOffset, 0),
            transform.rotation
        );
        NetworkServer.Spawn(projectile);

        projectileTransform = projectile.transform;

        float Vx = Mathf.Sqrt(projectileVelocity);

        float flightDuration = throwDistance / Vx;

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            projectileTransform.Translate(0, 0, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;

            yield return null;
        }

        Destroy(projectile);
    }

    [Command]
    void CmdThrowProjectile(string toSpawn)
    {
        StartCoroutine(ThrowProjectileFromPlayer(toSpawn));
    }
}
