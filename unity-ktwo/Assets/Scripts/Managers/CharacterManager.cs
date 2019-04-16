using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For clients to load up characters at runtime.
public class CharacterManager : MonoBehaviour
{   
    public static CharacterManager instance;
    public List<GameObject> characters;
    string characterURI = "Resources/Characters/{0}";

    void Awake()
    {
        instance = this;
        // characters.Add((GameObject)Resources.Load(string.Format(characterURI, "Architect"), typeof(GameObject)));
        // characters.Add((GameObject)Resources.Load(string.Format(characterURI, "Chemist"), typeof(GameObject)));
        // characters.Add((GameObject)Resources.Load(string.Format(characterURI, "Chef"), typeof(GameObject)));
        // characters.Add((GameObject)Resources.Load(string.Format(characterURI, "Dog"), typeof(GameObject)));
        // characters.Add((GameObject)Resources.Load(string.Format(characterURI, "Tinkerer"), typeof(GameObject)));
    }
}
