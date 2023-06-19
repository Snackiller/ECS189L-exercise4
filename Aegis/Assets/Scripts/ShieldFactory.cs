using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ShieldFactory : MonoBehaviour
{
    public class ShieldSpec
    {
        public float Capacity { get; set; }
        public float RechargeRate { get; set; }
        public float RechargeDelay { get; set; }
        public EffectTypes Type { get; set; }
    }

    [SerializeField] private GameObject shieldPrefab;

    private float CalculateShieldRating(ShieldSpec spec)
    {
        return spec.Capacity + (5 - spec.RechargeDelay) * 5 + spec.RechargeRate * (spec.RechargeRate / 2);
    }

    private ShieldSpec ScaleDownShieldSpec(ShieldSpec spec)
    {
        while (CalculateShieldRating(spec) > 300.0f)
        {
            spec.Capacity *= 0.5f;
            spec.RechargeRate *= 0.5f;
            spec.RechargeDelay *= 1.5f;
        }
        return spec;
    }

    public GameObject Build(ShieldSpec spec)
    {
        if (CalculateShieldRating(spec) > 300.0f)
        {
            spec = ScaleDownShieldSpec(spec);
        }

        GameObject shield = Instantiate(this.shieldPrefab, this.transform.position, Quaternion.identity);
        ShieldController controller = shield.GetComponent<ShieldController>();
        controller.capacity = spec.Capacity;
        controller.rechargeRate = spec.RechargeRate;
        controller.rechargeDelay = spec.RechargeDelay;
        controller.type = spec.Type;
        return shield;
    }

    public GameObject GenerateRandomShield()
    {
        ShieldSpec spec = new ShieldSpec
        {
            Capacity = Random.Range(50.0f, 250.1f),
            RechargeRate = Random.Range(1.0f, 25.1f),
            RechargeDelay = Random.Range(0.5f, 5.1f),
            Type = (EffectTypes)Random.Range(0, 3)
        };
        return Build(spec);
    }
}