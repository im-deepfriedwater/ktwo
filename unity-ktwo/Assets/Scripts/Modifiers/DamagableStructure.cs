using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DamagableStructure : Damagable
{
    public float health = 100; // 100 by default.
    public bool isInvincible = false;
    public Material invincibleMaterial;

    // There must be a network structure on the root
    // structure group.
    public NetworkStructure networkStructure;
    
    private Material defaultMaterial;

    new void Start ()
    {
        networkStructure = transform.parent.gameObject.GetComponent<NetworkStructure>();
        defaultMaterial = gameObject.GetComponent<Renderer>().material;
        base.Start();
    }

    override public void Hit(float damage)
    {
        if (isInvincible || !networkStructure.isServer)
        {
            return;
        }

        networkStructure.RpcTellStructureHit(damage);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public IEnumerator BeginInvincibility(float duration)
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
        if (!networkStructure.isServer) return;
        NetworkServer.Destroy(transform.parent.gameObject);
    }
}
