using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinicibilityBehaviour : MonoBehaviour
{
    PlayerBehaviour player;
    float duration = 2; // 2 by default;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
