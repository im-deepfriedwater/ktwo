using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(InvinicibilityFlashModifier))]
public class DamagableEnemy: Damagable
{   
    public float health = 100; // 100 by default.
    [Range(1, 10)]
    public int knockbackFactor; // There's not really any sense of units here sorry...
    private float calculatedKnockBackFactor;
    public float invincibilityDuration = 2; // 2 by default;
    public Rigidbody rbd;
    public bool isInvincible = false;

    [Tooltip("These are populated at runtime")]
    public new UnityEventFloat OnHit; // `new` Overrides original OnHit field.

    private GameObject fireEffect;

    InvinicibilityFlashModifier invinicibilityComponent;

    new void Start()
    {
        if (OnHit == null) OnHit = new UnityEventFloat();
        fireEffect = GameObject.Find("CFX4 Fire");
        fireEffect.SetActive(false);
        rbd = GetComponent<Rigidbody>();
        invinicibilityComponent = GetComponent<InvinicibilityFlashModifier>();
        base.Start();
    }

    override public void Hit(float damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        OnHit.Invoke(currentHealth / health);
        StartCoroutine(BeginInvincibility());
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public IEnumerator DamageOverTime(float damageAmount, float duration)
    {
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Hit(damageAmount);
            yield return new WaitForSeconds(1.0f);
            elapsedTime++;
        }
    }

    public IEnumerator SetOnFire(float duration)
    {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        fireEffect.SetActive(false);
    }

    public void ClientSideHit(float damage, Vector3 direction)
    {
        if (isServer) return;
        CmdReportEnemyHit(damage);
        Hit(damage);
        KnockbackEnemy(direction);
        StartCoroutine(BeginInvincibility());
    }

    public void ServerSideHit(float damage, Vector3 direction)
    {
        if (!isServer) return;
        RpcTellEnemyHit(damage);
        Hit(damage);
        KnockbackEnemy(direction);
        StartCoroutine(BeginInvincibility());
    }

    void KnockbackEnemy(Vector3 direction)
    {
        calculatedKnockBackFactor = knockbackFactor * 10; // Adjusted from 1 - 10 -> 10 - 100
        rbd.AddForce(direction.normalized * calculatedKnockBackFactor, ForceMode.VelocityChange);
    }

    IEnumerator BeginInvincibility()
    {   
        isInvincible = true;
        invinicibilityComponent.enabled = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        invinicibilityComponent.enabled = false;
    }

    public void Die()
    {
        if (isServer) 
        {
            EnemyManager.instance.zombies.Remove(gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }

    [Command]
    public void CmdReportEnemyHit(float damage)
    {
        health -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [ClientRpc]
    public void RpcTellEnemyHit(float damage)
    {
        Hit(damage);
    }
}
