using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public struct AbilityUseParams {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage) {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;

        protected ISpecialAbility behaviour; // Only methods or derived classes can access

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams useParams) {
            behaviour.Use(useParams);
        }

        public float GetEnergyCost() {
            return energyCost;
        }

        public GameObject GetParticlePrefab() {
            return particlePrefab;
        }

    } // SpecialAbilityConfig

    public interface ISpecialAbility {
        void Use(AbilityUseParams useParams);
    } // ISpecialAbility
}
