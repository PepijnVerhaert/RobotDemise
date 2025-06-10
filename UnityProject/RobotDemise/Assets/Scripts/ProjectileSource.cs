using UnityEngine;

public interface IProjectileSource
{
    public abstract bool ProjectileHit(BaseProjectile.ProjectileInfo projectileInfo, Collider collider);
    public abstract void LifeEnd(BaseProjectile.ProjectileInfo projectileInfo);
}
