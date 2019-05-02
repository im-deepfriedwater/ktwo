using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// This object goes on the in-game character
// prefab. Assign the icons in the inspector
// and they will show up in-game when the
// encounter begins.
public class SetCharacterUI : MonoBehaviour
{
    public List<Sprite> abilityIcons;
    public Sprite characterPortrait;

    string abilityIconFormat = "AbilityIcon{0}";
    string characterPortraitName = "CharacterPortrait";

    // Needs to be called after the PlayersAbilityUI is enabled
    // otherwise GameObject.Find will fail.
    public void Initialize()
    {
        var go = GameObject.Find(characterPortraitName);
        go.GetComponent<Image>().sprite = characterPortrait;
        for (int i = 0; i < abilityIcons.Count; i++)
        {
            go = GameObject.Find(string.Format(abilityIconFormat, i));
            go.GetComponent<Image>().sprite = abilityIcons[i];
        }
    }
}
