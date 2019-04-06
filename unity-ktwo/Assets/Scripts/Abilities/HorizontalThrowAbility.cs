using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalThrowAbility : AbstractAbility
{
    public string abilityName;
    public float heightOffset;
    public float throwDistance; // Distance to spawn from player.
    public float projectileVelocity;
    public GameObject projectilePrefab;

    protected virtual void Update()
    {
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

        var projectile = Instantiate(projectilePrefab, playerTransform.position + playerTransform.forward + new Vector3(0, heightOffset, 0), playerTransform.rotation);
        var projectileTransform = projectile.transform;

        float vx = Mathf.Sqrt(projectileVelocity);

        float flightDuration = throwDistance / vx;

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            projectileTransform.Translate(0, 0, vx * Time.deltaTime);
            elapse_time += Time.deltaTime;

            yield return null;
        }

        Destroy(projectile);
    }
}
