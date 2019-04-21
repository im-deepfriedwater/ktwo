using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpfulPuddleBehaviour : BasePuddleBehaviour
{
    public float speedBoostPercent;
    public float buffDuration;
    public float speedDebuffPercent;
    public float debuffDuration;
    public float invincibilityDuration;
    public float DPSBuffPercent;
    public float DPSBuffDuration;

    void OnTriggerEnter(Collider other)
    {
        if (CannotBeUsed(other.gameObject)) return;

        if (other.gameObject.tag == "Player")
        {
            affectedEntities.Add(other.gameObject);
            other.GetComponent<PlayerBehaviour>().RpcTimedAffectSpeed(speedBoostPercent, buffDuration, true);
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

        if (other.gameObject.tag == "Zombie")
        {
            affectedEntities.Add(other.gameObject);
            StartCoroutine(
                other.GetComponent<EnemyController>()
                    .TimedAffectSpeed(speedDebuffPercent, debuffDuration, false)
            );
            StartCoroutine(
                RemoveFromHashSet(other.gameObject, debuffDuration)
            );
            numberOfUses -= 1;
        }
    }
}
