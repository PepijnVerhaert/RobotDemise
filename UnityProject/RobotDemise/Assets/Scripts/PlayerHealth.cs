using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private BaseRobot _robot;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            var enemy = collision.collider.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy == null) return;
            bool lethal = _robot.GetHit(enemy.EnemyStats.damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy == null) return;
            bool lethal = _robot.GetHit(enemy.EnemyStats.damage);
        }
    }
}
