using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyGame;

public class HelloWorld : MonoBehaviour
{
    void Start()
    {
        Player cc = new Player();

        print(cc.health + "/" + cc.maxHealth);

        print("Health " + cc.CurrentHealthPercent() + "%");

    }
}
