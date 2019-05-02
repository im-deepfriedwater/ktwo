using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(InvinicibilityFlashModifier))]
public class DamagableEnemy : Damagable
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

    override public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, startingHealth);

        RpcHeal(healAmount);
    }

    [ClientRpc]
    public void RpcHeal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, startingHealth);
        OnHit.Invoke(currentHealth / startingHealth);
    }

    public void Hit(float damage, Vector3 direction, bool iFrames)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (currentHealth <= 0) Die();

        if (iFrames)
        {
            KnockbackEnemy(direction);
            StartCoroutine(BeginInvincibility());
        }

        RpcHit(damage, direction, iFrames);
    }

    [ClientRpc]
    public void RpcHit(float damage, Vector3 direction, bool iFrames)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        if (currentHealth <= 0) Die();

        if (iFrames)
        {
            KnockbackEnemy(direction);
            StartCoroutine(BeginInvincibility());
        }
    }

    public IEnumerator DamageOverTime(float damageAmount, float duration)
    {
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            Hit(damageAmount, Vector3.zero, false);
            yield return new WaitForSeconds(1.0f);
            elapsedTime++;
        }
    }

    [ClientRpc]
    public void RpcSetOnFire(float duration)
    {
        StartCoroutine(SetOnFire(duration));
    }

    public IEnumerator SetOnFire(float duration)
    {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(duration);
        fireEffect.SetActive(false);
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
            WaveManager.instance.OnZombieDeath();
            NetworkServer.Destroy(gameObject);
        }
    }
}
