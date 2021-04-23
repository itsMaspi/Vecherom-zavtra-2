using UnityEngine;

public interface IProjectileWeapon
{
    Transform ProjectileSpawn { get; set; }
    GameObject CastProjectile();
}
