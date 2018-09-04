using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";

        public abstract void Use(GameObject target);

        public void SetConfig(AbilityConfig configToSet) {
            config = configToSet;
        }

        protected void PlayParticleEffect() {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

            particleObject.transform.parent = transform; // Set world space in prefab if required
            ParticleSystem myParticleSystem = particleObject.GetComponent<ParticleSystem>();
            myParticleSystem.Play();

            float totalDuration = myParticleSystem.main.duration + myParticleSystem.main.startLifetime.constant; // Fixes duration bug

            Destroy(particleObject, totalDuration);
        }

        protected void PlayAbilityAnimation() {
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
            var animator = GetComponent<Animator>();

            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected void PlayAbilitySound() {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }

    } // AbilityBehaviour
}
