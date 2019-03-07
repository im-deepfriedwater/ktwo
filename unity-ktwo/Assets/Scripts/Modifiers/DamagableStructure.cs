using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagableStructure : Damagable
{
    public float health = 100; // 100 by default.
    public bool isInvincible = false;
    public Material invincibleMaterial;
    
    private Material defaultMaterial;

    new void Start ()
    {
        defaultMaterial = gameObject.GetComponent<Renderer>().material;
        base.Start();
    }

    override public void Hit (float damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            OnZeroHealth.Invoke();
        }
        Debug.Log("current health " + currentHealth);
    }

    public IEnumerator BeginInvincibility (float duration)
    {   
        var materialRenderer = gameObject.GetComponent<Renderer>();
        isInvincible = true;
        materialRenderer.material = invincibleMaterial;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
        materialRenderer.material = defaultMaterial;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
