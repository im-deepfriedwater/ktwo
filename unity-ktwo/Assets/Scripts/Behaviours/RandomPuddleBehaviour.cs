﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        buff = (Random.Range(0, 2) == 0);
        gameObject.GetComponentInChildren<Renderer>().material = buff ? buffPuddle : debuffPuddle;
        base.Awake();
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
                    .TimedAffectSpeed(speedBoostPercent, buffDuration, true, affectedEntities)
            );
            other.GetComponent<DamagablePlayer>().Heal(healAmount);
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            // -15% run speed for 5 seconds
            StartCoroutine(
                other.GetComponent<EnemyController>()
                    .TimedAffectSpeed(speedBoostPercent, buffDuration, true, affectedEntities)
            );
            other.GetComponent<DamagableEnemy>().Heal(healAmount);
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);

            var structureDamagable = other.gameObject.GetComponent<DamagableStructure>();
            if (structureDamagable != null)
            {
                structureDamagable.Heal(structureHealAmount);
                StartCoroutine(
                    structureDamagable.BeginInvincibility(invincibilityDuration)
                );
            }

            var dpsMod = other.gameObject.GetComponent<DPSModifier>();
            if (dpsMod != null)
            {
                StartCoroutine(
                    dpsMod.AffectDPS(DPSBuffPercent, DPSBuffDuration, true)
                );
            }
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
                    .TimedAffectSpeed(speedDebuffPercent, debuffDuration, false, affectedEntities)
            );
            StartCoroutine(
                other.GetComponent<DamagablePlayer>()
                    .DamageOverTime(DPS, DOTDuration, affectedEntities)
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<EnemyController>()
                    .TimedAffectSpeed(speedDebuffPercent, debuffDuration, false, affectedEntities)
            );
            StartCoroutine(
                other.GetComponent<DamagableEnemy>()
                    .DamageOverTime(DPS, DOTDuration, affectedEntities)
            );
            numberOfUses -= 1;
        }
        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);

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
            }
            numberOfUses -= 1;
        }
    }
}
