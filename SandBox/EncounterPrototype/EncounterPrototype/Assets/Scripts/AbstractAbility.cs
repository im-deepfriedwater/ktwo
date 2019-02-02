using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractAbility : MonoBehaviour
{
    public Buttons[] inputButtons;
    public MonoBehaviour[] disableScripts;
    public float cooldown;
    [Range(0, 3)]
    public int abilitySlot;

    protected InputState inputState;
    protected Image abilityIcon;
    protected bool cooldownOver = true;
    protected float cooldownPercentage = 0;

    protected Image abilityGroupImage; // Used as a border behind the ability icon.

    protected void Initialize()
    {
        abilityGroupImage = GameObject
            .Find(string.Format("AbilityGroup{0}", abilitySlot))
            .GetComponent<Image>();

        abilityIcon = GameObject
            .Find(string.Format("AbilityIcon{0}", abilitySlot))
            .GetComponent<Image>();

        Debug.Log(abilityIcon.name);
    }

    #region ability icon UI methods

    private void MarkAbilityAsReady()
    {
        abilityGroupImage.enabled = true;
    }

    private void MarkAbilityAsUsed()
    {
        abilityGroupImage.enabled = false;

    }

    private void UpdateCooldownMask()
    {
        abilityIcon.fillAmount = cooldownPercentage; // The range is from 0 - 1
    }

    protected void UpdateAbilityUI() // This should be called at some point in the derived's update method
    {
        if (cooldownOver)
        {
            MarkAbilityAsReady();
        }
        else
        {
            // Commented out right now because it looks better without it.
            // MarkAbilityAsUsed();
            UpdateCooldownMask();
        }
    }

    #endregion

    IEnumerator WaitForCooldown()
    {
        var timeSinceLastPressed = 0f;

        while (timeSinceLastPressed < cooldown)
        {
            timeSinceLastPressed += Time.deltaTime;
            cooldownPercentage = timeSinceLastPressed / cooldown;
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
