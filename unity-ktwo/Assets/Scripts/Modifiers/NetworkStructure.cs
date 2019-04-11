using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkStructure: NetworkBehaviour
{
    public DamagableStructure structure;

    void Start()
    {
        structure = GetComponentInChildren<DamagableStructure>();
    }

    [ClientRpc]
    public void RpcTellStructureHit(float damage)
    {
        structure.health -= damage;
        if (structure.currentHealth <= 0)
        {
            structure.Die();
        }
    }
}
