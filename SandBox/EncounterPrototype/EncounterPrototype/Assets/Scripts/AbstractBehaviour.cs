using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBehaviour : MonoBehaviour
{

    public Buttons[] inputButtons;
    public MonoBehaviour[] disableScripts;
    public float cooldown;

    protected InputState inputState;
    protected bool cooldownOver = true;


    IEnumerator WaitForCooldown()
    {
        float timeSinceLastPressed = 0;

        while (timeSinceLastPressed < cooldown)
        {
            timeSinceLastPressed += Time.deltaTime;
            yield return null;
        }

        cooldownOver = true;
    }

    protected virtual void Awake()
    {
        inputState = GetComponent<InputState>();
    }

    protected void ToggleScripts(bool value)
    {
        foreach (var script in disableScripts)
        {
            script.enabled = value;
        }
    }
}
