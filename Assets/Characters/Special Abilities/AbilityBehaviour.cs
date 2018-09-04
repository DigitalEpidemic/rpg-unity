using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

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

        protected void PlayAbilitySound() {
            var abilitySound = config.GetRandomAbilitySound(); // TODO Change to random clip
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }

    } // AbilityBehaviour
}
