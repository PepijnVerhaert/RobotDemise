using UnityEngine;

public interface IProjectileSource
{
    public abstract bool ProjectileHit(BaseProjectile projectile, Collider collider); //return destroy projectile
    public abstract void LifeEnd(BaseProjectile projectile);
}
