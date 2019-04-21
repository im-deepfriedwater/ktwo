using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CSSCharacterIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CharacterEnum character;
    public Color active = new Color(1, 1, 1);
    public Color inactive = new Color(0.25f, 0.25f, 0.25f);
    public bool playable = true;
    Image icon;

    void Start()
    {
        icon = GetComponent<Image>();
        
        if (!playable) icon.color = inactive;
    }

    public void OnPointerEnter(PointerEventData _)
    {
        CSSManager.instance.PreviewCharacter(character);
    }

    public void OnPointerExit(PointerEventData _)
    {
        CSSManager.instance.RestoreChosenCharacter();
    }

    public void OnPointerClick(PointerEventData _)
    {
        if (!playable) return;
        CSSManager.instance.ChooseCharacter(character);
    }
}
