using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPuddleBehaviour : MonoBehaviour
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

    [Header("Number of Uses")]
    public int numberOfUses;

    [Header("Materials")]
    public Material buffPuddle;
    public Material debuffPuddle;

    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();
    private bool buff;
        
    void Awake()
    {
        buff = (Random.Range(0, 2) == 0);
        gameObject.GetComponentInChildren<Renderer>().material = buff ? buffPuddle : debuffPuddle;
    }

    void Update()
    {
        if (numberOfUses == 0 && affectedEntities.Count == 0) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (numberOfUses == 0) return;
        if (affectedEntities.Contains(other.gameObject)) return;

        if (buff) 
            Buff(other);
        else
            Debuff(other);
    }

    private void Buff(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<PlayerBehaviour>().
                TimedAffectSpeed(speedBoostPercent, buffDuration, true, affectedEntities)
            );
            other.GetComponent<DamagablePlayer>().Heal(healAmount);
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            // -15% run speed for 5 seconds
            StartCoroutine(
                other.GetComponent<EnemyController>().
                TimedAffectSpeed(speedBoostPercent, buffDuration, true, affectedEntities)
            );
            other.GetComponent<DamagableEnemy>().Heal(healAmount);
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);
            // TO DO
            numberOfUses -= 1;
        }
    }

    private void Debuff(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<PlayerBehaviour>().
                TimedAffectSpeed(speedDebuffPercent, debuffDuration, false, affectedEntities)
            );
            StartCoroutine(
                other.GetComponent<DamagablePlayer>().
                DamageOverTime(DPS, DOTDuration, affectedEntities)
            );
            numberOfUses -= 1;
        }

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<EnemyController>().
                TimedAffectSpeed(speedDebuffPercent, debuffDuration, false, affectedEntities)
            );
            StartCoroutine(
                other.GetComponent<DamagableEnemy>().
                DamageOverTime(DPS, DOTDuration, affectedEntities)
            );
            numberOfUses -= 1;
        }
        if (other.gameObject.tag == "Structure")
        {
            affectedEntities.Add(other.gameObject);
            // TO DO
            numberOfUses -= 1;
        }
    }
}
