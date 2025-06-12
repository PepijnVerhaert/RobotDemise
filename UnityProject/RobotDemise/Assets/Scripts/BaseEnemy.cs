using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Stats _stats = new Stats { health = 10f, speed = 3.5f, damage = 1f };
    public Stats EnemyStats { get { return _stats; } }

    public struct Stats
    {
        public float speed;
        public float health;
        public float damage;
    }

    public bool Damage(float amount)
    {
        _stats.health -= amount;

        if(_stats.health <= 0f)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
