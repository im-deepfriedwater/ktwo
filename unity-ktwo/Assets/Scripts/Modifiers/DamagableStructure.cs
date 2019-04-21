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
    public NetworkRoot network;

    private Material defaultMaterial;

    new void Start()
    {
        network = transform.parent.gameObject.GetComponent<NetworkRoot>();
        defaultMaterial = gameObject.GetComponent<Renderer>().material;
        base.Start();
    }

    override public void Hit(float damage)
    {
        if (isInvincible || !network.isServer)
        {
            return;
        }

        network.RpcTellStructureHit(damage);
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
        if (!network.isServer) return;
        NetworkServer.Destroy(transform.parent.gameObject);
    }

    [ClientRpc]
    public void RpcHeal(float healAmount)
    {
        Heal(healAmount);
    }
}
