using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RandomPuddleBehaviour : BasePuddleBehaviour
{
    [Header("Player/Zombie BUFF Settings")]
    public float speedBoostPercent;
    public float buffDuration;
    public float healAmount;

    [Header("Player/Zombie DEBUFF Settings")]
    public float speedDebuffPercent;
    public float debuffDuration;
    public float DPS;
    public float DOTDuration;

    [Header("Structure BUFF Settings")]
    public float DPSBuffPercent;
    public float DPSBuffDuration;
    public float invincibilityDuration;
    public float structureHealAmount;

    [Header("Structure DEBUFF Settings")]
    public float DPSDebuffPercent;
    public float DPSDebuffDuration;
    public float structureDamageAmount;

    [Header("Materials")]
    public Material buffPuddle;
    public Material debuffPuddle;

    private bool buff;

    new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (isServer)
        {
            buff = (Random.Range(0, 2) == 0);
            gameObject.GetComponentInChildren<Renderer>().material = buff ? buffPuddle : debuffPuddle;
            RpcSetBuff(buff);
        }
    }

    [ClientRpc]
    void RpcSetBuff(bool buff)
    {
        this.buff = buff;
        gameObject.GetComponentInChildren<Renderer>().material = buff ? buffPuddle : debuffPuddle;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject)) return;

        if (buff)
        {
            Buff(other);
        }
        else
        {
            Debuff(other);
        }
    }

    private void Buff(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<PlayerBehaviour>()
                    .TimedAffectSpeed(speedBoostPercent, buffDuration, true)
            );
            other.GetComponent<DamagablePlayer>().RpcHeal(healAmount);
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, buffDuration)
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<EnemyController>()
                    .TimedAffectSpeed(speedBoostPercent, buffDuration, true)
            );
            other.GetComponent<DamagableEnemy>().RpcHeal(healAmount);
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, buffDuration)
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);

            var overallDuration = 0f;
            var structureDamagable = other.gameObject.GetComponent<DamagableStructure>();
            if (structureDamagable != null)
            {
                structureDamagable.RpcHeal(structureHealAmount);
                StartCoroutine(
                    structureDamagable.BeginInvincibility(invincibilityDuration)
                );
                overallDuration = invincibilityDuration;
            }

            var dpsMod = other.gameObject.GetComponent<DPSModifier>();
            if (dpsMod != null)
            {
                StartCoroutine(
                    dpsMod.AffectDPS(DPSBuffPercent, DPSBuffDuration, true)
                );
                overallDuration = (overallDuration < DPSBuffDuration) ? DPSBuffDuration : overallDuration;
            }

            StartCoroutine(
                RemoveFromHashSet(other.gameObject, overallDuration)
            );
            numberOfUses -= 1;
        }
    }

    private void Debuff(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<PlayerBehaviour>()
                    .TimedAffectSpeed(speedDebuffPercent, debuffDuration, false)
            );
            StartCoroutine(
                other.GetComponent<DamagablePlayer>()
                    .DamageOverTime(DPS, DOTDuration)
            );
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, Mathf.Max(debuffDuration, DOTDuration))
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<EnemyController>()
                    .TimedAffectSpeed(speedDebuffPercent, debuffDuration, false)
            );
            StartCoroutine(
                other.GetComponent<DamagableEnemy>()
                    .DamageOverTime(DPS, DOTDuration)
            );
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, Mathf.Max(debuffDuration, DOTDuration))
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);

            var overallDuration = 0f;
            var structureDamagable = other.gameObject.GetComponent<DamagableStructure>();
            if (structureDamagable != null)
            {
                structureDamagable.Hit(structureDamageAmount);
            }

            var dpsMod = other.gameObject.GetComponent<DPSModifier>();
            if (dpsMod != null)
            {
                StartCoroutine(
                    dpsMod.AffectDPS(DPSDebuffPercent, DPSDebuffDuration, false)
                );
                overallDuration = DPSDebuffDuration;
            }
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, overallDuration)
            );
            numberOfUses -= 1;
        }
    }
}
