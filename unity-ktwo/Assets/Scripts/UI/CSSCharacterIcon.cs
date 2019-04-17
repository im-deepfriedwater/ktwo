using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSCharacterIcon : MonoBehaviour
{
    public CharacterEnum character;

    void OnMouseEnter()
    {
        CSSManager.instance.PreviewCharacter(character);
    }

    void OnMouseExit()
    {
        CSSManager.instance.RestoreChosenCharacter();
    }

    void OnMouseUp()
    {
        CSSManager.instance.ChooseCharacter(character);
    }
}
