using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aegis
{
    [CreateAssetMenu]
    public class EffectTypeColors : ScriptableObject
    {
        [SerializeField]
        private Color kineticColor;
        
        [SerializeField]
        private Color energyColor;

        [SerializeField]
        private Color arcaneColor;

        public Color GetColorByEffectType(EffectTypes type)
        {
            switch (type) 
            {
                case EffectTypes.Kinetic:
                    return this.kineticColor;
                case EffectTypes.Energy:
                    return this.energyColor;
                case EffectTypes.Arcane:
                    return this.arcaneColor;
                default:
                    Debug.Log("No valid EffectType found. Returning white");
                    return Color.white;
            }
        }
    }
}