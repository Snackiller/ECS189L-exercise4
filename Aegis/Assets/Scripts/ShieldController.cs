using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ShieldController : MonoBehaviour   
{
    [SerializeField] public float capacity = 100.0f;
    [SerializeField] public float rechargeRate = 1.0f;
    [SerializeField] public float rechargeDelay = 1.0f;
    [SerializeField] public EffectTypes type = EffectTypes.Kinetic;
    [SerializeField] private EffectTypeColors effectTypeColors;
    [SerializeField] private GameObject scrollingText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private float currentCapacity = 0.0f;
    [SerializeField] private ShieldFactory shieldFactory;
    private HealthBarController healthBarController;
    private float timeSinceLastDamage = 0.0f;
    private float timeSinceRechargeDelay = 0.0f;

    public static class DamageEngine
    {
        private static float GetTypeFactor(EffectTypes ProjectileType, EffectTypes ShieldType)
        {
            if (ProjectileType == EffectTypes.Kinetic)
            {
                if (ShieldType == EffectTypes.Kinetic)
                {
                    return 1.0f;
                }
                else if (ShieldType == EffectTypes.Energy)
                {
                    return 0.5f;
                }
                else if (ShieldType == EffectTypes.Arcane)
                {
                    return 2.0f;
                }
            }
            else if (ProjectileType == EffectTypes.Energy)
            {
                if (ShieldType == EffectTypes.Kinetic)
                {
                    return 2.0f;
                }
                else if (ShieldType == EffectTypes.Energy)
                {
                    return 1.0f;
                }
                else if (ShieldType == EffectTypes.Arcane)
                {
                    return 0.5f;
                }
            }
            else if (ProjectileType == EffectTypes.Arcane)
            {
                if (ShieldType == EffectTypes.Kinetic)
                {
                    return 0.5f;
                }
                else if (ShieldType == EffectTypes.Energy)
                {
                    return 2.0f;
                }
                else if (ShieldType == EffectTypes.Arcane)
                {
                    return 1.0f;
                }
            }
            return 1.0f;
        }
        public static float CalculateDamage(float ProjectileDamage, EffectTypes ProjectileType, EffectTypes ShieldType)
        {
            float typeFactor = GetTypeFactor(ProjectileType, ShieldType);
            return ProjectileDamage * typeFactor;
        }
    }

    void Awake()
    {
        this.currentCapacity = this.capacity;   

        if (!this.healthBar.TryGetComponent<HealthBarController>(out this.healthBarController))
        {
            Debug.Log("ShieldController expects a health bar.");
        }

        this.healthBarController.ChangeValue(this.currentCapacity / this.capacity);

        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(this.type));     
    }

    private void TakeDamage(float damage)
    {
        float oldCapacity = this.currentCapacity;
        this.currentCapacity -= damage;
        
        if (currentCapacity < 0.0f)
        {
            currentCapacity = 0.0f;
            GameObject randomShield = shieldFactory.GenerateRandomShield();
            Destroy(this.gameObject);

        }
        if (currentCapacity <= 0.0f && oldCapacity > 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Explode");
        }
        else if (currentCapacity > 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Shrink");
        }

        this.healthBarController.ChangeValue(currentCapacity / capacity);
        
        if(this.scrollingText && oldCapacity > 0)
        {
            this.ShowScrollingText(damage.ToString());
        }
        this.timeSinceLastDamage = 0.0f;
    }

    private void ShowScrollingText(string message)
    {
        var scrollingText = Instantiate(this.scrollingText, this.transform.position, Quaternion.identity);
        scrollingText.GetComponent<TextMesh>().text = message;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Projectile" == other.tag)
        {
            //var damage = other.GetComponent<ProjectileController>().GetDamage();
            var projectileController = other.GetComponent<ProjectileController>();
            var damage = DamageEngine.CalculateDamage(projectileController.GetDamage(), projectileController.GetEffectType(), this.type);
            TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var capacityRatio = currentCapacity / capacity;
        this.transform.localScale = new Vector3(capacityRatio, capacityRatio, capacityRatio);
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", effectTypeColors.GetColorByEffectType(this.type));
        if (currentCapacity < capacity)
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage > rechargeDelay)
            {
                timeSinceRechargeDelay += Time.deltaTime;
                if (timeSinceRechargeDelay > 1.0f / rechargeRate)
                {
                    currentCapacity += rechargeRate * Time.deltaTime;
                    if (currentCapacity > capacity)
                    {
                        currentCapacity = capacity;
                    }
                    this.healthBarController.ChangeValue(currentCapacity / capacity);
                }
            }
        }
        else
        {
            timeSinceLastDamage = 0.0f;
            timeSinceRechargeDelay = 0.0f;
        }
    }

    private void Start()
    {
        this.healthBarController.ChangeValue(currentCapacity / capacity);
        timeSinceLastDamage = 0.0f;
        timeSinceRechargeDelay = 0.0f;
    }
}
