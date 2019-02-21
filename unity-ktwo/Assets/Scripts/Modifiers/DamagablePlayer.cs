using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InvinicibilityFlashModifier))]
public class DamagablePlayer: Damagable
{   
    public float health = 100; // 100 by default.
    public float knockbackScale = 2;
    public float invincibilityDuration = 2; // 2 by default;
    [Tooltip("These are populated at runtime")]
    public new UnityEventFloat OnHit; // `new` Overrides original OnHit field.
    public Rigidbody rbd;

    public bool isInvincible = false;

    InvinicibilityFlashModifier invinicibilityComponent;
    
    void Awake ()
    {
        rbd = GetComponent<Rigidbody>();
        if (OnHit == null)
        {
            OnHit = new UnityEventFloat();
        }
        OnHit.AddListener(GetComponent<PlayerBehaviour>().UpdateHealthBar);
    }

    new void Start ()
    {
        invinicibilityComponent = GetComponent<InvinicibilityFlashModifier>();
        base.Start();
    }

    override public void Hit (float damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        OnHit.Invoke(currentHealth / health);
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }
    }

    // A hit with knockback.
    public void Hit (float damage, Vector3 direction)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        OnHit.Invoke(currentHealth / health);
        KnockbackPlayer(direction);
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }

        StartCoroutine("BeginInvincibility");
    }

    void KnockbackPlayer (Vector3 direction)
    {
        Debug.Log("pushed");
        rbd.AddForce(direction * 1000, ForceMode.Acceleration);
    }

    IEnumerator BeginInvincibility ()
    {   
        isInvincible = true;
        invinicibilityComponent.enabled = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        invinicibilityComponent.enabled = false;
    }
}
