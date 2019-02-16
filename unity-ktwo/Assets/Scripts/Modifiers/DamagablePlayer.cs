using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagablePlayer: Damagable
{   
    public float health = 100; // 100 by default.
    public float knockbackScale = 2;
    public new UnityEventFloat OnHit; // Overrides original OnHit field.
    public Rigidbody rbd;
    
    void Awake ()
    {
        rbd = GetComponent<Rigidbody>();
        if (OnHit == null)
        {
            OnHit = new UnityEventFloat();
        }
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
        Debug.Log("PUSHED");
        Vector3 randomForce = new Vector3(10, 10, 10);
        // rbd.AddForce(direction.normalized * 100);
        rbd.AddForce(randomForce);
    }
}
