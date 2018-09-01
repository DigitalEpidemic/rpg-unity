using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet) {
            this.config = configToSet;
        }


        public void Use(AbilityUseParams useParams) {
            DealDamage(useParams);
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams useParams) {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.AdjustHealth(damageToDeal);
        }

        private void PlayParticleEffect() {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            // TODO Decide if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

    } // PowerAttackBehaviour
}
