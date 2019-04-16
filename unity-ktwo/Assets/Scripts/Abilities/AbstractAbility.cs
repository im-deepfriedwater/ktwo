using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public abstract class AbstractAbility : NetworkBehaviour
{
    public Buttons[] inputButtons;
    public float cooldown;

    [Range(0, 3)]
    public int abilitySlot;

    protected InputState inputState;
    protected Image abilityIcon;
    protected bool cooldownOver = true;
    protected float cooldownPercentage = 0;

    protected Image abilityGroupImage; // Used as a border behind the ability icon.

    protected PlayerBehaviour player;

    protected void Initialize()
    {
        if (!hasAuthority) return;

        abilityGroupImage = GameObject
            .Find(string.Format("AbilityGroup{0}", abilitySlot))
            .GetComponent<Image>();

        abilityIcon = GameObject
            .Find(string.Format("AbilityIcon{0}", abilitySlot))
            .GetComponent<Image>();
    }

    void Start() // Will get called automatically on startup.
    {
        Initialize();
        player = GetComponent<PlayerBehaviour>();
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
        if (!hasAuthority) return;

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

    [Command]
    public void CmdBuildObject(string name, Vector3 position, Quaternion rotation)
    {
        var go = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)), position, rotation);
        NetworkServer.Spawn(go);
    }

    [Command]
    public void CmdBuildObjectWithPrefab(GameObject prefab)
    {
        var go = (GameObject)Instantiate(Resources.Load(name, typeof(GameObject)), prefab.transform.position, prefab.transform.rotation);
        NetworkServer.Spawn(go);
    }
}
