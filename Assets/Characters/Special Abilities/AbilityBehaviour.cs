using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public abstract class AbilityBehaviour : MonoBehaviour {

        protected AbilityConfig config;

        public abstract void Use(AbilityUseParams useParams);

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

    } // AbilityBehaviour
}
