using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CharacterInfoStruct
{
    public CharacterEnum character;

    public string characterDescription;

   public  GameObject previewObject;

    // These both should have a 1-to-1
    // correspondence and be length 4.
    public string[] abilityDescriptions;
    public Image[] abilityIcons;
}
