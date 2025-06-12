using UnityEngine;

public sealed class MinigunWeapon : BaseWeapon, IProjectileSource
{
    private float _lifeTime = .5f;
    private float _projectileSpeed = 20f;
    private float _baseDamage = 1f;
    private float _baseCooldown = 1f;

    private float _damagePerLevel = .5f;
    private float _cooldownPerLevel = -.15f;

    private float _currentProjectileSpeed = 20f;
    private float _currentDamage = 1f;
    private float _currentCooldown = 1f;
    private float _currentCritChance = 0f;
    private int _currentPiercing = 0;

    private float _timer = 0f;

    //stats
    private int _bulletsFired = 0;
    private int _enemiesHit = 0;
    private int _enemiesKilled = 0;
    private float _damageDone = 0;
    private float _criticalDamageDone = 0;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSource;

    public override void ActivateEffect(string effectName)
    {
        switch (effectName)
        {
            case "LevelUp":
                _level += 1;
                ActivateEffect("UpdateStats");
                break;
            case "UpdateStats":
                var stats = _robot.GetStats();
                _currentProjectileSpeed = _projectileSpeed * stats.projectileSpeed;
                _currentDamage = (_baseDamage + (_level * _damagePerLevel)) * stats.damage;
                _currentCooldown = (_baseCooldown + (_level * _cooldownPerLevel)) * stats.haste;
                _currentCritChance = stats.luck;
                _currentPiercing = stats.projectilePiercing;
                break;
            default:
                break;
        }
        return;
    }

    public void LifeEnd(BaseProjectile projectile)
    {
    }

    public bool ProjectileHit(BaseProjectile projectile, Collider collider)
    {
        var minigunProjectile = projectile as MinigunBullet;
        if (collider.tag == "Enemy")
        {
            var enemy = collider.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy == null)
            {
                Debug.Log("[MinigunWeapon.cs][ProjectileHit] hit enemy without health");
                return false;
            }
            var damage = _currentDamage;
            if (minigunProjectile._isCrit)
            {
                _currentDamage *= 2f;
                _criticalDamageDone += damage;
            }
            _damageDone += damage;
            bool killedEnemy = enemy.Damage(damage);
            if (killedEnemy) _enemiesKilled++;
            _enemiesHit += projectile.Info.hits;
        }
        else if (collider.tag == "Terrain")
        {
            return true;
        }
        
        return projectile.Info.hits > _currentPiercing;
    }

    private void Start()
    {
        _name = "Minigun";
        ActivateEffect("UpdateStats");
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _currentCooldown * _robot.GetStats().haste) return;

        _timer = 0f;

        var direction = (_robot._mouseTarget - _robot.transform.position).normalized;
        direction.y = 0f;
        GameObject bulletObject = Instantiate(_bulletPrefab, _bulletSource.position, Quaternion.LookRotation(direction));

        var bullet = bulletObject.GetComponent<MinigunBullet>();
        bullet._source = this;
        bullet._maxLifetime = _lifeTime;
        bullet._speed = _projectileSpeed;
        bullet._direction = direction;
        bullet._isCrit = Random.Range(0f, 1f) < _currentCritChance;

        _bulletsFired++;
        //Debug.Log("[MinigunWeapon.cs][Update] fire bullet " + _bulletsFired);
    }
}
