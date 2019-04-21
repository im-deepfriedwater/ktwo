using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CSSCharacterInfoStruct
{
    public CharacterEnum character;

    [TextArea(4, 4)]
    public string characterDescription;

    public GameObject previewModel;

    [TextArea(4, 4)]
    // These both should have a 1-to-1
    // correspondence and be length 4.
    public string[] abilityDescriptions;
    public Sprite[] abilityIcons;
}
