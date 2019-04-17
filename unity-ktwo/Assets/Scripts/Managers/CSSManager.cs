using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSManager : MonoBehaviour
{
    public static CSSManager instance;
    CharacterEnum chosenCharacter;
    public List<CharacterInfoStruct> rosterData;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PreviewCharacter(CharacterEnum characterToPreview)
    {
        throw new System.NotImplementedException();
    }

    public void RestoreChosenCharacter()
    {
        throw new System.NotImplementedException();
    }

    public void ChooseCharacter(CharacterEnum characterToChoose)
    {
        throw new System.NotImplementedException();
    }
}
