using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CSSAbilityIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int slotNumber;

    public void OnPointerEnter(PointerEventData _)
    {
        CSSManager.instance.ShowAbilityDescription(slotNumber);
    }

    public void OnPointerExit(PointerEventData _)
    {
        CSSManager.instance.RestoreCharacterDescription();
    }
}
