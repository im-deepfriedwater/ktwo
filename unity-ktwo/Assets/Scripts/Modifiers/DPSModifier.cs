using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSModifier : MonoBehaviour
{
    public float damageAmount;

    private float baseDamage;

    void Start()
    {
        baseDamage = damageAmount;
    }

    public IEnumerator AffectDPS(float percent, float time, bool buff)
    {
        var change = baseDamage * percent;
        damageAmount = buff ? damageAmount + change : damageAmount - change;
        yield return new WaitForSeconds(time);
        ResetDPS();
    }

    private void ResetDPS()
    {
        damageAmount = baseDamage;
    }

}
