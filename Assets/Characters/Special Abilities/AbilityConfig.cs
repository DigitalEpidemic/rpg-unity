using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

    public abstract class AbilityConfig : ScriptableObject {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour; // Only methods or derived classes can access

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo) {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target) {
            behaviour.Use(target);
        }

        public float GetEnergyCost() {
            return energyCost;
        }

        public GameObject GetParticlePrefab() {
            return particlePrefab;
        }

        public AudioClip GetRandomAbilitySound() {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        public AnimationClip GetAbilityAnimation() {
            RemoveAnimationEvents();
            return abilityAnimation;
        }

        // Removes bugs/crashes from asset packs
        void RemoveAnimationEvents() {
            abilityAnimation.events = new AnimationEvent[0];
        }

    } // AbilityConfig
}
