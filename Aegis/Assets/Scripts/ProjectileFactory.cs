using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ProjectileFactory : MonoBehaviour
{
    public class ProjectileSpec
    {
        public float Damage { get; set; }
        public float ChargeDelay { get; set; }
        public EffectTypes Type { get; set; }
    }

    [SerializeField] private GameObject projectilePrefab;

    private float CalculateProjectileRating(ProjectileSpec spec)
    {
        return spec.Damage * 2 + (3 - spec.ChargeDelay) * (3 - spec.ChargeDelay) * (3 - spec.ChargeDelay) * (3 - spec.ChargeDelay);
    }

    private ProjectileSpec ScaleDownProjectileSpec(ProjectileSpec spec)
    {
        while (CalculateProjectileRating(spec) > 100.0f)
        {
            spec.Damage *= 0.5f;
            spec.ChargeDelay *= 1.5f;
        }
        return spec;
    }

    public GameObject Build(ProjectileSpec spec)
    {
        if (CalculateProjectileRating(spec) > 100.0f)
        {
            spec = ScaleDownProjectileSpec(spec);
        }

        GameObject projectile = Instantiate(this.projectilePrefab);
        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        controller.damage = spec.Damage;
        controller.chargeDelay = spec.ChargeDelay;
        controller.type = spec.Type;
        return projectile;
    }

    public GameObject GenerateRandomProjectile()
    {
        ProjectileSpec spec = new ProjectileSpec
        {
            Damage = Random.Range(1.0f, 50.1f),
            ChargeDelay = Random.Range(0.5f, 3.1f),
            Type = (EffectTypes)Random.Range(0,3)
        };
        return Build(spec);
    }
}