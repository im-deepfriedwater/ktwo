using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour: MonoBehaviour
{
    public void Break () // Should be called when there is 0 health left;
    {
        Debug.Log("Lol");
        Destroy(gameObject); // TODO should start playing a breakdown animation AND then destroy
    }
}
