using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThrowAbility : AbstractAbility
{
    public string abilityName;
    public float heightOffset;
    public float throwDistance; // Distance to spawn from player.
    public GameObject projectilePrefab;
    public GameObject projectileRemnantPrefab;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    private Transform projectileTransform;


    // Update is called once per frame
    protected virtual void Update()
    {
        if (player.isDead) return;

        if (inputState.GetButtonValue(inputButtons[0]) && cooldownOver)
        {
            cooldownOver = false;
            StartCoroutine("WaitForCooldown");
            StartCoroutine(ThrowProjectileFromPlayer());
        }
        UpdateAbilityUI();
    }

    IEnumerator ThrowProjectileFromPlayer()
    {
        var Projectile = Instantiate(
            projectilePrefab,
            transform.position + transform.forward + new Vector3(0, heightOffset, 0),
            transform.rotation
        );
        CmdBuildObjectWithPrefab(Projectile);

        projectileTransform = Projectile.transform;

        var newThrowDistance = throwDistance + Mathf.Tan((90 - firingAngle) * Mathf.Deg2Rad) * heightOffset;

        float projectileVelocity = newThrowDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = (newThrowDistance + heightOffset) / Vx;

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            projectileTransform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;

            yield return null;
        }

        // I haven't done the calculation for the parabolitic projectile motion so i hardcoded height for now
        var puddlePosition = new Vector3(Projectile.transform.position.x, player.transform.position.y, Projectile.transform.position.z);
        CmdBuildObject(projectileRemnantPrefab.name, puddlePosition, Projectile.transform.rotation);
        NetworkServer.Destroy(Projectile);
    }
}
