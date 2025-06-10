using UnityEngine;

public sealed class MinigunWeapon : BaseWeapon, IProjectileSource
{
    private float _lifeTime = 1f;
    private float _projectileSpeed = 20f;
    private float _baseDamage = 1f;
    private float _baseCooldown = 1f;

    private float _damagePerLevel = .5f;
    private float _cooldownPerLevel = -.15f;

    private float _currentProjectileSpeed = 20f;
    private float _currentDamage = 1f;
    private float _currentCooldown = 1f;
    private float _currentCritChance = 0f;

    private float _timer = 0f;

    //stats
    private int _bulletsFired = 0;
    private int _enemiesHit = 0;
    private int _enemiesKilled = 0;
    private float _damageDone = 0;
    private float _criticalDamageDone = 0;

    [SerializeField] private GameObject _bulletPrefab;

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
                break;
            default:
                break;
        }
        return;
    }

    public void LifeEnd(BaseProjectile.ProjectileInfo projectileInfo)
    {
    }

    public bool ProjectileHit(BaseProjectile.ProjectileInfo projectileInfo, Collider collider)
    {
        _enemiesHit += projectileInfo.hits;
        //damge if hit was enemy
        return true;
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
        GameObject bulletObject = Instantiate(_bulletPrefab, _robot.transform.position, Quaternion.LookRotation(direction));

        var bullet = bulletObject.GetComponent<MinigunBullet>();
        bullet._source = this;
        bullet._maxLifetime = _lifeTime;
        bullet._speed = _projectileSpeed;
        bullet._direction = direction;

        _bulletsFired++;
        Debug.Log("fire bullet " + _bulletsFired);
    }
}
