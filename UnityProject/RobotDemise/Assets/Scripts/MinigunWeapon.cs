using UnityEngine;

public sealed class MinigunWeapon : BaseWeapon
{
    private float _lifeTime = 1f;
    private float _projectileSpeed = 20f;
    private float _baseDamage = 1f;
    private float _baseCooldown = 1f;

    private float _damageMultPerLevel = 1.25f;
    private float _cooldownMultPerLevel = .9f;

    private float _currentDamage = 1f;
    private float _currentCooldown = 1f;

    private float _timer = 0f;

    //stats
    private int _bulletsFired = 0;
    private int _bulletsHit = 0;
    private int _enemiesHit = 0;
    private int _enemiesKilled = 0;
    private float _damageDone = 0;
    private float _criticalDamageDone = 0;

    private GameObject _bulletPrefab;

    public override void ActivateEffect(string effectName)
    {
        switch (effectName)
        {
            case "LevelUp":
                _level += 1;
                _currentDamage *= _damageMultPerLevel;
                _currentCooldown *= _cooldownMultPerLevel;
                break;
            default:
                break;
        }
        return;
    }

    private void Start()
    {
        _name = "Minigun";
        _currentDamage = _baseDamage;
        _currentCooldown = _baseCooldown;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _currentCooldown * _robot.GetStats().haste) return;

        _timer = 0f;
        //fire bullet
        _bulletsFired++;
        Debug.Log("fire bullet " + _bulletsFired);
    }
}
