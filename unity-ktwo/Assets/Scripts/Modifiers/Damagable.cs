using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public float startingHealth = 100;
    public float currentHealth;
    public UnityEvent OnZeroHealth; // Death animations, or functions to call on death go here.
    public UnityEvent OnHit;

    protected void Start()
    {
        currentHealth = startingHealth;

        if (OnZeroHealth == null)
        {
            OnZeroHealth = new UnityEvent();
        }
    }

    virtual public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, startingHealth);
    }

    virtual public void Hit (float damage) 
    {
        currentHealth -= damage;
        OnHit.Invoke();
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }
    }
}
