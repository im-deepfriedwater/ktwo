using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePlayer: Damagable
{   
    public float health = 100; // 100 by default.
    public float knockbackScale = 2;
    public float invincibilityDuration = 2; // 2 by default;
    public new UnityEventFloat OnHit; // Overrides original OnHit field.
    public Rigidbody rbd;
    
    void Awake ()
    {
        rbd = GetComponent<Rigidbody>();
        if (OnHit == null)
        {
            OnHit = new UnityEventFloat();
        }
        OnHit.AddListener(GetComponent<PlayerBehaviour>().UpdateHealthBar);
    }

    override public void Hit (float damage)
    {
        currentHealth -= damage;
        OnHit.Invoke(currentHealth / health);
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }
    }

    public void Hit (float damage, Vector3 direction)
    {
        currentHealth -= damage;
        OnHit.Invoke(currentHealth / health);
        KnockbackPlayer(direction);
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }
    }

    void KnockbackPlayer (Vector3 direction)
    {
        Debug.Log("pushed");
        rbd.AddForce(direction * 1000, ForceMode.Acceleration);
    }
}
