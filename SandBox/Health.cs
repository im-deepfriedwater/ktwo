
namespace MyGame
{
    public class Health
    {
        public int health = 5;
        public int maxHealth = 10;

        public float CurrentHealthPercent()
        {
            return (health / (float)maxHealth) * 100;
        }

        public virtual void RestoreHealth()
        {
            health = maxHealth;
        }
    }
}
