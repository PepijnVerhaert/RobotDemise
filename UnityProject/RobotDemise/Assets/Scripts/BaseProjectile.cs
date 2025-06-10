using System;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
	public struct ProjectileInfo
	{
        public int hits;
	}

    private ProjectileInfo _info = new ProjectileInfo { hits = 0 };

	public IProjectileSource _source { private get; set; }

    public float _maxLifetime = -1f;
    private float _lifeTimer = 0f;

    protected virtual void FixedUpdate()
    {
        if (_maxLifetime > 0f)
        {
            _lifeTimer += Time.deltaTime;
            if (_lifeTimer >= _maxLifetime)
            {
                _source.LifeEnd(_info);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _info.hits++;

        bool destroy = _source.ProjectileHit(_info, collision.collider);

        if(destroy) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        _info.hits++;

        bool destroy = _source.ProjectileHit(_info, other);

        if (destroy) Destroy(gameObject);
    }
}
