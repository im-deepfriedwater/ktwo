using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CharacterInfoStruct
{
    CharacterEnum character;

    string characterDescription;

    GameObject previewObject;

    // These both should have a 1-to-1
    // correspondence and be length 4.
    string[] abilityDescriptions;
    Image[] abilityIcons;
}
