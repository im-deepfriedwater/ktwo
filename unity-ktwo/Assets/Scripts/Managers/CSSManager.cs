using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSManager : MonoBehaviour
{
    public static CSSManager instance;

    CharacterEnum chosenCharacter;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
