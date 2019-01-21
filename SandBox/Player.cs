using System;
using UnityEngine;

namespace MyGame
{
    public class Player : Health
    {
        public Player()
        {
            RestoreHealth();
        }

        override public void RestoreHealth()
        {

            for (int i = health; i <= maxHealth; i++)
            {
                health = i;
                Debug.Log("Health " + health);
            }

        }
    }
}